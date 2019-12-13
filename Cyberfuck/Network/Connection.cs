using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using Cyberfuck.Data;

namespace Cyberfuck.Network
{
    enum ConnectionState
    {
        Connected,
        Initialized,
        NotConnected
    }
    class Connection
    {
        ConnectionState state;
        public TcpClient conn;
        public NetworkStream stream;
        public readonly int id;

        public bool Connected { get => state == ConnectionState.Connected; }
        public ConnectionState State { get => state; set => state = value; }
        public NetworkStream Stream { get => stream; set => stream = value; }

        public Connection(TcpClient conn)
        {
            conn.NoDelay = true;
            this.conn = conn;
            stream = conn.GetStream();
            state = ConnectionState.Initialized;
        }
        public Connection(int id)
        {
            this.id = id;
            state = ConnectionState.NotConnected;
        }

        /// <summary>
        /// check if any new data from the client is avalible and process it
        /// the oposite process of the SendMessage method
        /// </summary>
        public void Update()
        {
            NetworkMessage message;
            MessageError error;
            while((error = NetworkMessage.TryDecodeFromStream(out message, stream)) == MessageError.Success)
            {
                // only posibility i can think of is to do a switch case for each message content type and
                // then decoding the message content to the appropriate data and process it.
                switch (message.Type)
                {
                    case MessageContentType.PlayerUpdate:
                        PlayerData playerData = PlayerData.Decode(message.Content);
                        if(World.players.ContainsKey(playerData.ID))
                            World.players[playerData.ID].Apply(playerData);
                        else
                        {
                            World.LoadPlayer(playerData, false);
                        }
                        break;
                    case MessageContentType.EntityData:
                        break;
                    case MessageContentType.RemovePlayer:
                        RemovePlayerData data = RemovePlayerData.Decode(message.Content);
                        World.RemovePlayer(data.playerId);
                        break;
                    default:
                        break;
                }
            }
        }
        
        public void Reset(TcpClient conn)
        {
            conn.NoDelay = true;
            state = ConnectionState.Initialized;
            this.conn = conn;
            this.stream = conn.GetStream();
        }

        public void Close()
        {
            this.Stream?.Close();
            this.conn?.Close();
            this.State = ConnectionState.NotConnected;
        }
    }
}
