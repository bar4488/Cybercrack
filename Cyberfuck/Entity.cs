using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Cyberfuck
{
    abstract class Entity
    {
        public RectangleF Bounds;

        public Vector2 Center { get => Bounds.Center; }
        public Vector2 Position { get => Bounds.Location; }

        public Entity()
        {
            Bounds = new RectangleF(0, 0, 0, 0);
        }
        public Entity(RectangleF bounds)
        {
            this.Bounds = bounds;
        }
        public Entity(Vector2 position, Point size)
        {
            this.Bounds = new RectangleF(position.X, position.Y, size.X, size.Y);
        }

        public abstract void Draw(SpriteBatch spriteBatch);

        public abstract void Update();
    }
}
