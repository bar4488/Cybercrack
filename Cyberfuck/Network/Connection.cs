using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using Cyberfuck.Data;
using Cyberfuck.GameWorld;

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
        World world;

        public bool Connected { get => state == ConnectionState.Connected; }
        public ConnectionState State { get => state; set => state = value; }
        public NetworkStream Stream { get => stream; set => stream = value; }

        public Connection(World world, TcpClient conn)
        {
            this.world = world;
            conn.NoDelay = true;
            this.conn = conn;
            stream = conn.GetStream();
            state = ConnectionState.Initialized;
        }
        public Connection(World world, int id)
        {
            this.world = world;
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
                        if(world.Players.ContainsKey(playerData.ID))
                            world.Players[playerData.ID].Apply(playerData);
                        else
                        {
                            world.LoadPlayer(playerData, false);
                        }
                        break;
                    case MessageContentType.AddTile:
                        AddTileData tileData = AddTileData.Decode(message.Content);
                        world.Map.AddTile(tileData.x, tileData.y, (Tile)tileData.tile);
                        break;
                    case MessageContentType.InventoryUpdate:
                        InventoryData inventoryData = InventoryData.Decode(message.Content);
                        world.Players[inventoryData.playerId].Apply(inventoryData);
                        break;
                    case MessageContentType.UseItem:
                        UseItemData useItemData = UseItemData.Decode(message.Content);
                        if(world.NetType == NetType.Server)
                            CyberFuck.netPlay.SendMessage(MessageContentType.UseItem, id, useItemData);
                        var player = world.Players[useItemData.player];
                        switch (useItemData.useType)
                        {
                            case 0:
                                player.Use(useItemData.mousePosition);
                                break;
                            case 1:
                                player.SecondUse(useItemData.mousePosition);
                                break;
                            default:
                                break;
                        }
                        break;
                    case MessageContentType.EntityData:
                        break;
                    case MessageContentType.RemovePlayer:
                        RemovePlayerData data = RemovePlayerData.Decode(message.Content);
                        world.RemovePlayer(data.playerId);
                        break;
                    default:
                        throw new NotImplementedException();
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
