using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cyberfuck.Entities;
using Cyberfuck.Network;

namespace Cyberfuck.Data
{
    public class PlayerData: IMessageContent
    {
        public const int ContentLength = EntityData.ContentLength + sizeof(int);
        public EntityData Entity;
        public int ID;

        public MessageContentType ContentType => MessageContentType.PlayerUpdate;

        public PlayerData(EntityData entityData, int id)
        {
            this.Entity = entityData;
            this.ID = id;
        }
        public PlayerData(Player player)
        {
            Entity = new EntityData(player);
            ID = player.ID;
        }
        public static bool operator ==(PlayerData d, PlayerData o)
        {
            return d.Entity == o.Entity;
        }

        public static bool operator !=(PlayerData d, PlayerData o)
        {
            return !(d.Entity==o.Entity);
        }
        public static bool operator ==(PlayerData d, Player o)
        {
            return d.Entity == o;
        }

        public static bool operator !=(PlayerData d, Player o)
        {
            return !(d.Entity==o);
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public byte[] Encode()
        {
            return ByteManipulation.WrapWithInt(this.Entity.Encode(), this.ID);
        }

        public static PlayerData Decode(byte[] data)
        {
            byte[] entityData = new byte[EntityData.ContentLength];
            Buffer.BlockCopy(data, ContentLength - EntityData.ContentLength, entityData, 0, EntityData.ContentLength);
            return new PlayerData(EntityData.Decode(entityData), BitConverter.ToInt32(data, 0));
        }
    }
}
