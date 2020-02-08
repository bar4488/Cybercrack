using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Cyberfuck.GameWorld;

namespace Cyberfuck.Damage
{
    public enum DamageReason
    {
        Fall,
        Hit,
    }
    class DamageField
    {
        Vector2 position;
        Vector2 velocity;
        Humper.IBox box;
        int damageAmount;
        public Vector2 Position { get => new Vector2(box.X, box.Y); }
        public Vector2 Velocity { get => velocity; set => velocity = value; }
        public Point PPosition { get => new Point((int)box.X, (int)box.Y); }
        public Point PVelocity { get => new Point((int)velocity.X, (int)velocity.Y); set => velocity = new Vector2(value.X, value.Y); }

        public int DamageAmount { get => damageAmount; set => damageAmount = value; }

        public DamageField(World world, Vector2 position, Vector2 velocity, Vector2 size)
        {
            this.position = position;
            this.velocity = velocity;
            box = world.CollisionWorld.Create(position.X, position.Y, size.X, size.Y);
        }

        public void Update(GameTime gameTime)
        {
            box.Move(position.X + velocity.X, position.Y + velocity.Y, (x) => new Humper.Responses.IgnoreResponse(x));
            position += velocity;
        }

    }
}
