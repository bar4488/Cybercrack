using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Drawing;
using System.Messaging;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using Cyberfuck.Entities;
using Cyberfuck.Data;

namespace Cyberfuck.Network
{
    public class NetServer: NetBase
    {
        public const int MaxClients = 256;

        ServerSnapshot previousSnapshot;
        Connection[] clients = new Connection[MaxClients];
        TcpListener listener;
        bool connected;
        public NetServer(int port)
        {
            CyberFuck.Logger.Log("starting server");
            listener = new TcpListener(IPAddress.Any, port);
            for(int i = 0; i < MaxClients; i++)
            {
                clients[i] = new Connection(i);
            }
            connected = true;
            listener.Start();
            ThreadPool.QueueUserWorkItem(new WaitCallback(ListenForClients));
        }
        public override void SendBuffer(byte[] msg, int excluded)
        {
            for (int i = 0; i < clients.Length; i++)
            {
                if(clients[i].Connected && i != excluded)
                {
                    clients[i].Stream.Write(msg, 0, msg.Length);
                }
            }
        }

        public override void SnapShot()
        {
            previousSnapshot = ServerSnapshot.Snapshot();
        }

        public override void Close()
        {
            listener.Stop();
            foreach (var client in clients)
            {
                client.stream.Close();
                client.conn.Close();
            }
            connected = false;
        }

        public override void Update()
        {
            // send data to clients based on the difference between this snapshot and the previous one
            ServerSnapshot snapshot = ServerSnapshot.Snapshot();
            foreach (var playerKV in snapshot.playersData)
            {
                if(previousSnapshot.playersData[playerKV.Key] != playerKV.Value)
                {
                    // the player data changed, notify all the clients
                    SendMessage(MessageContentType.PlayerUpdate, playerKV.Key, playerKV.Value);
                }
            }
            for (int i = 0; i < snapshot.entitiesData.Count; i++)
            {
                if(previousSnapshot.entitiesData[i] != snapshot.entitiesData[i])
                {
                    SendMessage(MessageContentType.EntityData, -1, snapshot.entitiesData[i]);
                }
            }
            // read if theres new data in each of the clients sockets
            foreach(Connection client in clients)
            {
                if(client.Connected)
                    client.Update();
            }
        }

        private void ListenForClients(object data)
        {
            CyberFuck.Logger.Log("listening for connections...");
            while (connected)
            {
                TcpClient client = listener.AcceptTcpClient();
                lock (clients)
                {
                    if(NetStatus.playersCount < MaxClients)
                    {
                        for (int i = 0; i < MaxClients; i++)
                        {
                            if (!clients[i].Connected)
                            {
                                clients[i].Reset(client);
                                // put the client in initialization state and start initializing in another thread
                                ThreadPool.QueueUserWorkItem(new WaitCallback(InitializeClientConnection), clients[i]);
                                //NetStatus.playersCount++;
                                break;
                            }
                        }
                    }
                    else
                    {
                        //TODO: send to client that the server is full
                    }
                }
            }
        }

        private void InitializeClientConnection(object data)
        {
            CyberFuck.Logger.Log("Network", "initializing new client");
            Connection conn = (Connection)data;
            NetworkStream stream = conn.stream;
            //send the world bitmap to the client:
            Bitmap worldBitmap = World.map.bitmap;
            BinaryFormatter formatter = new BinaryFormatter();
            using (MemoryStream memmory = new MemoryStream())
            {
                formatter.Serialize(memmory, worldBitmap);
                long size = memmory.Length;
                Console.WriteLine(size);
                byte[] bsize = BitConverter.GetBytes(size);
                byte[] image = memmory.ToArray();
                stream.Write(bsize, 0, bsize.Length);
                stream.Write(image, 0, image.Length);
            }
            //send entities data (not players):
            byte[][] entitiesBytes = new byte[World.entities.Count][];
            List<IEntity> entities = World.entities;
            for(int i = 0; i < entities.Count; i++)
            {
                entitiesBytes[i] = new EntityData(entities[i]).Encode();
            }
            byte[] entitiesData = ByteManipulation.ConcatByteArrays(entitiesBytes);
            // send the length of the entities data, send the number of entities and then send the entities themselves
            byte[] entitiesMsg = ByteManipulation.ConcatByteArrays(BitConverter.GetBytes(entitiesData.Length), BitConverter.GetBytes(entities.Count), entitiesData);
            stream.Write(entitiesMsg, 0, entitiesMsg.Length);

            //send all players data:
            List<Player> players = World.players.Values.ToList();

            byte[][] playersBytes = new byte[World.players.Count][];
            for(int i = 0; i < players.Count; i++)
            {
                playersBytes[i] = new PlayerData(players[i]).Encode();
            }
            byte[] playersData = ByteManipulation.ConcatByteArrays(playersBytes);
            // send the length of the players data, send the number of players and then send the players themselves
            byte[] playersMsg = ByteManipulation.ConcatByteArrays(BitConverter.GetBytes(playersData.Length), BitConverter.GetBytes(players.Count), playersData);
            stream.Write(playersMsg, 0, playersMsg.Length);
            // after aproval of the client, send it its position
            // the client will send 0xFFFFFFFF to approve the connection
            CyberFuck.Logger.Log("Network", "waiting for client's approval...");
            byte[] approval = new byte[4];
            stream.Read(approval, 0, 4);
            if (!approval.All((b) => b == 0xFF))
            {
                CyberFuck.Logger.Log("Network", "Client connection failed - bad approval message");
                return;
            }
            // create a player for the client and 
            // send the client its position 
            Player clietnPlayer = new Player(conn.id);
            byte[] clientsPlayerBytes = new PlayerData(clietnPlayer).Encode();
            stream.Write(BitConverter.GetBytes(clientsPlayerBytes.Length), 0, sizeof(int));
            stream.Write(clientsPlayerBytes, 0, clientsPlayerBytes.Length);
            CyberFuck.Logger.Log("Network", "player with id " + conn.id + " added");
            World.players[conn.id] = clietnPlayer;
            conn.State = ConnectionState.Connected;
        }
    }
}
