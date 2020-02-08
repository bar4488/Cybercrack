using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using static Cyberfuck.Data.ByteManipulation;
using Cyberfuck.Network;

namespace Cyberfuck.Data
{
    class UseItemData: IMessageContent
    {
        // left clieck, right click ...
        public int useType;
        public int player;
        public Vector2 mousePosition;
        public UseItemData(int playerId, int useType, Vector2 mousePosition)
        {
            this.player = playerId;
            this.useType = useType;
            this.mousePosition = mousePosition;
        }
        public byte[] Encode()
        {
            return ConcatByteArrays(ConvertIntsToBytes(player, useType), ConvertFloatsToBytes(mousePosition.X, mousePosition.Y));
        }

        public static UseItemData Decode(byte[] data)
        {
            int[] ints = ConvertBytesToInts(data, 0, 2);
            int playerId = ints[0];
            int useType = ints[1];
            float[] floats = ConvertBytesToFloats(data, 2 * sizeof(int), 2);
            return new UseItemData(playerId, useType, new Vector2(floats[0], floats[1]));
        }
    }
}
