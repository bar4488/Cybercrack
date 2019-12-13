using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Cyberfuck.Entities
{
    public interface IEntity
    {
        EntityType Type { get; }
        int ID { get; }
        Point Position { get; }
        Point Velocity { get; }
    }
}
