using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Cyberfuck.Data
{
    struct PVData
    {
        public Point Position;
        public Point Velocity;
        public PVData(Point position, Point velocity)
        {
            Position = position;
            Velocity = velocity;
        }
    }
    struct PVDataF
    {
        public Vector2 Position;
        public Vector2 Velocity;
        public PVDataF(Vector2 position, Vector2 velocity)
        {
            Position = position;
            Velocity = velocity;
        }
    }
}
