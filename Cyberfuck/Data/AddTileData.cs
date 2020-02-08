using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cyberfuck.Network;
using Cyberfuck.GameWorld;

namespace Cyberfuck.Data
{
    struct AddTileData: IMessageContent
    {
        public int x;
        public int y;
        public int tile;

        public AddTileData(int x, int y, Tile tile)
        {
            this.x = x;
            this.y = y;
            this.tile = (int)tile;
        }
        public MessageContentType ContentType => MessageContentType.RemovePlayer;

        public byte[] Encode()
        {
            byte[] data = new byte[sizeof(int) * 3];
            Buffer.BlockCopy(new int[] { x, y, tile }, 0, data, 0, sizeof(int) * 3);
            return data;
        }

        public static AddTileData Decode(byte[] bytes)
        {
            int[] data = ByteManipulation.ConvertBytesToInts(bytes);
            return new AddTileData(data[0], data[1], (Tile)data[2]);
        }
    }
}
