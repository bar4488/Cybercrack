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

namespace Cyberfuck.Network
{
    public class NetServer: NetBase
    {
        public const int MaxClients = 256;

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

        public NetType Type => NetType.Server;

        public override void Close()
        {
            connected = false;
        }

        public override void Update()
        {
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
                entitiesBytes[i] = GetEntityDataBytes(new EntityData(entities[i]));
            }
            byte[] entitiesData = ByteManipulation.ConcatByteArrays(entitiesBytes);
            // send the length of the entities data, send the number of entities and then send the entities themselves
            byte[] entitiesMsg = ByteManipulation.ConcatByteArrays(BitConverter.GetBytes(entitiesData.Length), BitConverter.GetBytes(entities.Count), entitiesData);

            //send all players data:
            List<Player> players = World.players.Values.ToList();

            byte[][] playersBytes = new byte[World.entities.Count][];
            for(int i = 0; i < players.Count; i++)
            {
                entitiesBytes[i] = GetPlayerDataBytes(new PlayerData(players[i]));
            }
            byte[] playersData = ByteManipulation.ConcatByteArrays(playersBytes);
            // send the length of the players data, send the number of players and then send the players themselves
            byte[] playersMsg = ByteManipulation.ConcatByteArrays(BitConverter.GetBytes(playersData.Length), BitConverter.GetBytes(players.Count), playersData);

            // after aproval of the client, send it its position
            // the client will send 0xFFFFFFFF to approve the connection
            byte[] approval = new byte[4];
            stream.Read(approval, 0, 4);
            if (!approval.All((b) => b == 0xFF))
            {
                CyberFuck.Logger.Log("Client connection failed - bad approval message");
                return;
            }
            // create a player for the client and 
            // send the client its position 
            Player clietnPlayer = new Player(conn.id);
            World.players[conn.id] = clietnPlayer;
        }
    }
}
