using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cyberfuck.Network;
using Cyberfuck.GameWorld;

namespace Cyberfuck.Data
{
    struct DropItemData: IMessageContent
    {
        public int playerID;
        public int itemSlot;

        public DropItemData(int playerId, int itemSlot)
        {
            this.playerID = playerId;
            this.itemSlot = itemSlot;
        }
        public MessageContentType ContentType => MessageContentType.AddTile;

        public byte[] Encode()
        {
            byte[] data = new byte[sizeof(int) * 2];
            Buffer.BlockCopy(new int[] { playerID, itemSlot }, 0, data, 0, sizeof(int) * 2);
            return data;
        }

        public static DropItemData Decode(byte[] bytes)
        {
            int[] data = ByteManipulation.ConvertBytesToInts(bytes);
            return new DropItemData(data[0], data[1]);
        }
    }
}
