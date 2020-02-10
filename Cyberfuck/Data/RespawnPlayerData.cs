using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Cyberfuck.Network;

namespace Cyberfuck.Data
{
    struct RespawnPlayerData: IMessageContent
    {
        public int player;
        public Point position;

        public RespawnPlayerData(int x, int y, int player)
        {
            this.player = player;
            this.position = new Point(x, y);
        }
        public MessageContentType ContentType => MessageContentType.RespawnPlayer;

        public byte[] Encode()
        {
            byte[] data = new byte[sizeof(int) * 3];
            Buffer.BlockCopy(new int[] { player, position.X, position.Y }, 0, data, 0, sizeof(int) * 3);
            return data;
        }

        public static RespawnPlayerData Decode(byte[] bytes)
        {
            int[] data = ByteManipulation.ConvertBytesToInts(bytes);
            return new RespawnPlayerData(data[1], data[2], data[0]);
        }
    }
}
