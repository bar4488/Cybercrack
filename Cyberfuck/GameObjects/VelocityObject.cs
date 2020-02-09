using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Humper;
using Microsoft.Xna.Framework;

namespace Cyberfuck.GameObjects
{
    class VelocityObject
    {
        public IBox Box;
        public Vector2 Velocity;
        public Vector2 Position => new Vector2(Box.X, Box.Y);
        public Vector2 Center => new Vector2(Box.X + Box.Width / 2, Box.Y + Box.Height / 2);
        public VelocityObject(IBox box, Vector2 velocity)
        {
            this.Box = box;
            this.Velocity = velocity;
        }
    }
}
