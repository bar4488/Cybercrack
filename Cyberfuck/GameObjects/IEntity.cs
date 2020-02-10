using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Cyberfuck.GameObjects
{
    public interface IEntity: IGameObject
    {
        EntityType Type { get; }
        int ID { get; }
        bool NPC { get; }
        Point Position { get; }
        Point Velocity { get; }
        Vector2 TilePosition { get; }
        Vector2 TileVelocity { get; }
        bool IsDead { get; }
        int Health { get; }
        int Width { get; }
        int Height { get; }
    }
}
