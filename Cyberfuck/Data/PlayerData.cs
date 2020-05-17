using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cyberfuck.GameObjects;
using Cyberfuck.Network;
using static Cyberfuck.Data.ByteManipulation;

namespace Cyberfuck.Data
{
    public struct PlayerFlags
    {
        public bool directionRight;

        public PlayerFlags(int flags)
        {
            directionRight = (flags & (1 << 0)) != 0;
        }

        public int Encode()
        {
            return directionRight ? 1 : 0;
        }

        public override int GetHashCode()
        {
            return Encode();
        }
    }
    public class PlayerData: IMessageContent
    {
        public const int ContentLength = EntityData.ContentLength + sizeof(int) * 2;
        public EntityData Entity;
        public int ID;
        public PlayerFlags flags;

        public MessageContentType ContentType => MessageContentType.PlayerUpdate;

        public PlayerData(EntityData entityData, int id)
        {
            this.Entity = entityData;
            this.ID = id;
        }
        public PlayerData(EntityData entityData, int id, PlayerFlags flags)
        {
            this.Entity = entityData;
            this.flags = flags;
            this.ID = id;
        }
        public PlayerData(Player player)
        {
            Entity = new EntityData(player);
            ID = player.ID;
            flags = new PlayerFlags();
            flags.directionRight = player.DirectionRight;
        }
        public static bool operator ==(PlayerData d, PlayerData o)
        {
            return d.Entity == o.Entity && d.flags.Encode() == o.flags.Encode() && d.ID == o.ID;
        }

        public static bool operator !=(PlayerData d, PlayerData o)
        {
            return !(d==o);
        }
        public static bool operator ==(PlayerData d, Player o)
        {
            return d == new PlayerData(o);
        }

        public static bool operator !=(PlayerData d, Player o)
        {
            return !(d==o);
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
            return WrapWithInt(WrapWithInt(this.Entity.Encode(), flags.Encode()), this.ID);
        }

        public static PlayerData Decode(byte[] data)
        {
            byte[] entityData = new byte[EntityData.ContentLength];
            Buffer.BlockCopy(data, ContentLength - EntityData.ContentLength, entityData, 0, EntityData.ContentLength);
            return new PlayerData(EntityData.Decode(entityData), BitConverter.ToInt32(data, 0), new PlayerFlags(BitConverter.ToInt32(data, sizeof(int))));
        }
    }
}
