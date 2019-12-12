using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Cyberfuck
{
    public struct EntityData
    {
        public EntityType Type;
        public Point Position;
        public Point Velocity;

        public EntityData(IEntity entity)
        {
            Type = entity.Type;
            Position = entity.Position;
            Velocity = entity.Velocity;
        }

        public static bool operator ==(EntityData d, IEntity o)
        {
            return d.Position == o.Position && d.Velocity == o.Velocity && d.Type == o.Type;
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
    }
}
