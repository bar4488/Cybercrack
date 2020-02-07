using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

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
        int damageAmount;
        public Vector2 Position { get => position; set => position = value; }
        public Vector2 Velocity { get => velocity; set => velocity = value; }
        public Point PPosition { get => new Point((int)position.X, (int)position.Y); set => position = new Vector2(value.X, value.Y); }
        public Point PVelocity { get => new Point((int)velocity.X, (int)velocity.Y); set => velocity = new Vector2(value.X, value.Y); }

        public int DamageAmount { get => damageAmount; set => damageAmount = value; }

        public DamageField(Vector2 position, Vector2 velocity)
        {
            this.position = position;
            this.velocity = velocity;
        }
        public DamageField(Point position, Point velocity)
        {
            this.PPosition = position;
            this.PVelocity = velocity;
        }

        public void Update(GameTime gameTime)
        {
            position += velocity;
        }

    }
}
