using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cyberfuck.Network;

namespace Cyberfuck.Data
{
    struct RemovePlayerData: IMessageContent
    {
        public int playerId;

        public RemovePlayerData(int player)
        {
            this.playerId = player;
        }
        public MessageContentType ContentType => MessageContentType.RemovePlayer;

        public byte[] Encode()
        {
            byte[] bytes = BitConverter.GetBytes(playerId);
            return bytes;
        }

        public static RemovePlayerData Decode(byte[] bytes)
        {
            int playerId = BitConverter.ToInt32(bytes, 0);
            return new RemovePlayerData(playerId);
        }
    }
}
