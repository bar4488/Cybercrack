using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Cyberfuck.Entities;
using Cyberfuck.Network;

namespace Cyberfuck.Data
{
    public struct EntityData: IMessageContent
    {
        public const int ContentLength = 6 * sizeof(int);

        public EntityType Type;
        public int Health;
        public Point Position;
        public Point Velocity;

        public MessageContentType ContentType => MessageContentType.EntityData;

        public EntityData(EntityType type, int health, Point position, Point velocity)
        {
            this.Health = health;
            this.Type = type;
            this.Position = position;
            this.Velocity = velocity;
        }
        public EntityData(IEntity entity)
        {
            this.Health = entity.Health;
            Type = entity.Type;
            Position = entity.Position;
            Velocity = entity.Velocity;
        }

        public static bool operator ==(EntityData d, EntityData o)
        {
            return d.Position == o.Position && d.Velocity == o.Velocity && d.Type == o.Type && d.Health == o.Health;
        }

        public static bool operator !=(EntityData d, EntityData o)
        {
            return !(d==o);
        }
        public static bool operator ==(EntityData d, IEntity o)
        {
            return d == new EntityData(o);
        }

        public static bool operator !=(EntityData d, IEntity o)
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
            int type = (int)this.Type;
            int posX = this.Position.X;
            int posY = this.Position.Y;
            int velX = this.Velocity.X;
            int velY = this.Velocity.Y;
            int[] dataArr = new int[] { type, Health, posX, posY, velX, velY };
            byte[] msg = new byte[dataArr.Length * sizeof(int)];
            Buffer.BlockCopy(dataArr, 0, msg, 0, msg.Length);
            return msg;
        }

        public static EntityData Decode(byte[] data)
        {
            int[] result = new int[data.Length / sizeof(int)];
            Buffer.BlockCopy(data, 0, result, 0, data.Length);
            return new EntityData((EntityType)result[0], result[1], new Point(result[2], result[3]), new Point(result[4], result[5]));
        }

    }
}
