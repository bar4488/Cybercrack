using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Humper;
using Humper.Responses;
using Microsoft.Xna.Framework.Input;

namespace Cyberfuck
{
    class Rigidbody
    {

        Vector2 velocity;
        IBox box;
        const int JUMP_VELOCITY = 24;
        const int MAX_SPEED = 8;
        const int FALL_SPEED = 12;
        const int gravity = 1;
        int mass = 100;

        public Vector2 Position { get => new Vector2(box.X, box.Y); }
        public Rectangle Bounds { get => new Rectangle((int)box.X, (int)box.Y, (int)box.Bounds.Width, (int)box.Bounds.Height);  }
        public Vector2 Velocity { get => velocity; set => velocity = value; }
        public Vector2 Acceleration { get; set; }
        public Vector2 Forces { get; set; }
        public void Update(GameTime gameTime)
        {
            Vector2 newAcceleration = Forces / mass;
            Vector2 newVelocity = Velocity + newAcceleration;
            Vector2 newPosition = Position + newVelocity;
            
            var move = box.Move(newPosition.X, newPosition.Y, (collision) =>
            {
                if (collision.Other.HasTag(Collider.Tile))
                {
                    return CollisionResponses.Slide;
                }
                return CollisionResponses.Cross;
            });
            foreach(IHit c in move.Hits)
            {
                if (c.Box.HasTag(Collider.Tile) && (c.Normal.Y != 0 && c.Box.Bounds.Left < Bounds.Right && c.Box.Bounds.Right > Bounds.Left))
                {
                    newVelocity.Y = 0;
                }
                if (c.Box.HasTag(Collider.Tile) && (c.Normal.X != 0 && c.Box.Bounds.Top < Bounds.Bottom && c.Box.Bounds.Bottom > Bounds.Top))
                {
                    newVelocity.X = 0;
                }
            }
            if (move.Hits.Any((c) => c.Box.HasTag(Collider.Tile) && (c.Normal.Y < 0 && c.Box.Bounds.Left < Bounds.Right && c.Box.Bounds.Right > Bounds.Left)))
            {
            }
        }
    }
}
