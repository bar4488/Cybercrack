using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Threading;
using System.Net.Sockets;
using Cyberfuck.Data;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Drawing;
using Cyberfuck.GameWorld;
using Microsoft.Xna.Framework;

namespace Cyberfuck.Network
{
    public delegate void OnConnectedEvent(World world);
    public class NetClient : NetBase
    {
        Connection server;
        ClientSnapshot previousSnapshot;
        public event OnConnectedEvent OnConnected;

        public NetClient(string ip, int port, string name): base(new World(name))
        {
            world.NetType = NetType.Client;
            TcpClient serverTcp = new TcpClient(ip, port);
            server = new Connection(world, serverTcp);
            ThreadPool.QueueUserWorkItem(new WaitCallback(Initialize), name);
        }

        public void Initialize(object data)
        {
            Tile[,] worldMap;
            CyberFuck.Logger.Log("Network", "initializing connection with server");
            NetworkStream stream = server.stream;
            string name = (string)data;
            byte[] byData = System.Text.Encoding.ASCII.GetBytes(name);
            stream.Write(new byte[] { (byte)byData.Length }, 0, 1);
            stream.Write(byData, 0, byData.Length);
            BinaryFormatter f = new BinaryFormatter();
            byte[] size = new byte[sizeof(long)];
            stream.Read(size, 0, sizeof(long));
            long isize = BitConverter.ToInt64(size, 0);
            byte[] imageBytes = new byte[isize];
            stream.Read(imageBytes, 0, (int)isize);
            using (MemoryStream memmoryStream = new MemoryStream(imageBytes))
            {
                memmoryStream.Position = 0;
                worldMap = (Tile[,])f.Deserialize(memmoryStream);
            }
            world.LoadWorld(worldMap);

            //read entities data
            byte[] header = new byte[sizeof(int) * 2];
            stream.Read(header, 0, header.Length);
            int dataLength = BitConverter.ToInt32(header, 0);
            int entitiesCount = BitConverter.ToInt32(header, sizeof(int));
            CyberFuck.Logger.Log("Network", "entities data length = " + dataLength);
            CyberFuck.Logger.Log("Network", "entities infered data length = " + entitiesCount * EntityData.ContentLength);
            if (dataLength != entitiesCount)
                throw new Exception("bad data length");
            List<EntityData> entitiesData = new List<EntityData>();
            for (int i = 0; i < entitiesCount; i++)
            {
                byte[] entityBytes = new byte[EntityData.ContentLength];
                stream.Read(entityBytes, 0, EntityData.ContentLength);
                entitiesData.Add(EntityData.Decode(entityBytes));
            }
            world.LoadEntities(entitiesData);


            //read players data
            header = new byte[sizeof(int) * 2];
            stream.Read(header, 0, header.Length);
            dataLength = BitConverter.ToInt32(header, 0);
            int playersCount = BitConverter.ToInt32(header, sizeof(int));
            CyberFuck.Logger.Log("Network", "players data length = " + dataLength);
            CyberFuck.Logger.Log("Network", "players infered data length = " + playersCount * PlayerData.ContentLength);
            List<PlayerData> playersData = new List<PlayerData>();
            for (int i = 0; i < playersCount; i++)
            {
                byte[] playersBytes = new byte[PlayerData.ContentLength];
                stream.Read(playersBytes, 0, PlayerData.ContentLength);
                playersData.Add(PlayerData.Decode(playersBytes));
                byte[] l = { 0 };
                stream.Read(l, 0, 1);
                byte[] buffer = new byte[l[0]];
                stream.Read(buffer, 0, l[0]);
                char[] chars = new char[l[0]];

                System.Text.Decoder d = System.Text.Encoding.UTF8.GetDecoder();
                int charLen = d.GetChars(buffer, 0, l[0], chars, 0);
                string playerName = new System.String(chars);
                world.LoadPlayer(playerName, PlayerData.Decode(playersBytes), false);
            }

            //send approval
            byte[] approval = new byte[] { 0xff, 0xff, 0xff, 0xff };
            stream.Write(approval, 0, approval.Length);

            // get my player details
            byte[] length = new byte[sizeof(int)];
            stream.Read(length, 0, length.Length);
            if (BitConverter.ToInt32(length, 0) != PlayerData.ContentLength)
                throw new Exception("bad player data length");
            byte[] playerBytes = new byte[PlayerData.ContentLength];
            stream.Read(playerBytes, 0, playerBytes.Length);
            PlayerData myPlayer = PlayerData.Decode(playerBytes);
            world.LoadPlayer(name, myPlayer, true);
            server.State = ConnectionState.Connected;
            OnConnected?.Invoke(world);
        }

        public override void SendBuffer(byte[] msg, int player)
        {
            if (server.State == ConnectionState.NotConnected)
                return;
            try
            {
                server.Stream.Write(msg, 0, msg.Length);
            }
            catch(System.IO.IOException e)
            {
                Close(CloseReason.ServerClosed);
            }
        }
        public override void Close(CloseReason reason)
        {
            server.stream.Close();
            server.conn.Close();
            server.State = ConnectionState.NotConnected;
            base.Close(reason);
        }
        public override void Update(GameTime gameTime)
        {
            if(!world.Player.IsDead)
            {
                ClientSnapshot snapshot = ClientSnapshot.SnapShot(world);
                if(previousSnapshot.playerData != snapshot.playerData)
                {
                    SendMessage(MessageContentType.PlayerUpdate, snapshot.playerData.ID, snapshot.playerData);
                }
            }
            // read new data from server
            server.Update();
        }
        public override void SnapShot()
        {
            previousSnapshot = ClientSnapshot.SnapShot(world);
        }
    }
}
