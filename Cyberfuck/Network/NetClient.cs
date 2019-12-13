﻿using System;
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
using Cyberfuck.Screen;

namespace Cyberfuck.Network
{
    public class NetClient : NetBase
    {
        Connection server;
        ClientSnapshot previousSnapshot;

        public NetClient(string ip, int port)
        {
            TcpClient serverTcp = new TcpClient(ip, port);
            server = new Connection(serverTcp);
            ThreadPool.QueueUserWorkItem(new WaitCallback(Initialize), 0);
        }

        public void Initialize(object data)
        {
            Bitmap worldMap;
            CyberFuck.Logger.Log("Network", "initializing connection with server");
            NetworkStream stream = server.stream;
            BinaryFormatter f = new BinaryFormatter();
            byte[] size = new byte[sizeof(long)];
            stream.Read(size, 0, sizeof(long));
            long isize = BitConverter.ToInt64(size, 0);
            byte[] imageBytes = new byte[isize];
            stream.Read(imageBytes, 0, (int)isize);
            using (MemoryStream memmoryStream = new MemoryStream(imageBytes))
            {
                memmoryStream.Position = 0;
                worldMap = (Bitmap)f.Deserialize(memmoryStream);
            }
            World.LoadWorld(worldMap);

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
            World.LoadEntities(entitiesData);


            //read players data
            header = new byte[sizeof(int) * 2];
            stream.Read(header, 0, header.Length);
            dataLength = BitConverter.ToInt32(header, 0);
            int playersCount = BitConverter.ToInt32(header, sizeof(int));
            CyberFuck.Logger.Log("Network", "players data length = " + dataLength);
            CyberFuck.Logger.Log("Network", "players infered data length = " + playersCount * PlayerData.ContentLength);
            if (dataLength != playersCount * PlayerData.ContentLength)
                throw new Exception("bad data length");
            List<PlayerData> playersData = new List<PlayerData>();
            for (int i = 0; i < playersCount; i++)
            {
                byte[] playersBytes = new byte[PlayerData.ContentLength];
                stream.Read(playersBytes, 0, PlayerData.ContentLength);
                playersData.Add(PlayerData.Decode(playersBytes));
            }
            World.LoadPlayers(playersData);

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
            World.LoadPlayer(myPlayer, true);
            server.State = ConnectionState.Connected;
            CyberFuck.Screen = new GameScreen();
        }

        public override void SendBuffer(byte[] msg, int player)
        {
            server.Stream.Write(msg, 0, msg.Length);
        }
        public override void Close()
        {
            server.stream.Close();
            server.conn.Close();
            server.State = ConnectionState.NotConnected;
        }
        public override void Update()
        {
            ClientSnapshot snapshot = ClientSnapshot.SnapShot();
            if(previousSnapshot.playerData != snapshot.playerData)
            {
                SendMessage(MessageContentType.PlayerUpdate, snapshot.playerData.ID, snapshot.playerData);
            }

            // read new data from server
            server.Update();
        }
        public override void SnapShot()
        {
            previousSnapshot = ClientSnapshot.SnapShot();
        }
    }
}