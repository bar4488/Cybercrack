using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cyberfuck.Network;
using Cyberfuck.GameObjects;

namespace Cyberfuck.Data
{
    public struct InventoryData : IMessageContent
    {
        public const int ContentLength = sizeof(int) * 12;

        public int playerId;
        public int selectItem;
        public int[] inventory;
        public InventoryData(int playerId, int selectItem, int[] inventory)
        {
            this.playerId = playerId;
            this.selectItem = selectItem;
            this.inventory = inventory;
            if (this.inventory.Length != 10)
                throw new IndexOutOfRangeException();
        }
        public InventoryData(Player player)
        {
            playerId = player.ID;
            selectItem = player.SelectedItem;
            inventory = new int[10];
            for (int i = 0; i < inventory.Length; i++)
            {
                if(player.Inventory[i] != null)
                    inventory[i] = player.Inventory[i].ItemID;
            }
        }
        public byte[] Encode()
        {
            byte[] inv = new byte[sizeof(int) * inventory.Length];
            Buffer.BlockCopy(inventory, 0, inv, 0, inv.Length);
            return ByteManipulation.WrapWithInt(ByteManipulation.WrapWithInt(inv, playerId), selectItem);
        }

        public static InventoryData Decode(byte[] data)
        {
            int[] ints = ByteManipulation.ConvertBytesToInts(data);
            int selectedItem = ints[0];
            int playerId = ints[1];
            int[] inv = ints.Skip(2).ToArray();
            return new InventoryData(playerId, selectedItem, inv);
        }

        public static bool operator ==(InventoryData d, InventoryData o)
        {
            return d.selectItem == o.selectItem && d.inventory.SequenceEqual(o.inventory);
        }

        public static bool operator !=(InventoryData d, InventoryData o)
        {
            return !(d == o);
        }

    }
}
