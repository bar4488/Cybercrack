using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Cyberfuck.Entities;

namespace Cyberfuck.Data
{
    public struct EntityData: IMessageContent
    {
        public const int ContentLength = 5 * sizeof(int);

        public EntityType Type;
        public Point Position;
        public Point Velocity;

        public MessageContentType ContentType => MessageContentType.EntityData;

        public EntityData(EntityType type, Point position, Point velocity)
        {
            this.Type = type;
            this.Position = position;
            this.Velocity = velocity;
        }
        public EntityData(IEntity entity)
        {
            Type = entity.Type;
            Position = entity.Position;
            Velocity = entity.Velocity;
        }

        public static bool operator ==(EntityData d, EntityData o)
        {
            return d.Position == o.Position && d.Velocity == o.Velocity && d.Type == o.Type;
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
            int[] dataArr = new int[] { type, posX, posY, velX, velY};
            byte[] msg = new byte[dataArr.Length * sizeof(int)];
            Buffer.BlockCopy(dataArr, 0, msg, 0, msg.Length);
            return msg;
        }

        public static EntityData Decode(byte[] data)
        {
            int[] result = new int[data.Length / 4];
            Buffer.BlockCopy(data, 0, result, 0, data.Length);
            return new EntityData((EntityType)result[0], new Point(result[1], result[2]), new Point(result[3], result[4]));
        }

    }
}
