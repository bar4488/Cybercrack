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
        public int DamageAmount { get => damageAmount; set => damageAmount = value; }

        public DamageField(Vector2 position, Vector2 velocity)
        {
            this.position = position;
            this.velocity = velocity;
        }

    }
}
