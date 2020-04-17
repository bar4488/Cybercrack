using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cyberfuck.GameWorld
{
    class WorldGen
    {
        public const int worldSize = 500;
        public const int worldHeight = 200;
        public int seed = 1;
        public int[] heights = new int[worldSize];
        FastNoise noise;
        public WorldGen()
        {
            noise = new FastNoise();
            noise.SetNoiseType(FastNoise.NoiseType.Perlin);
            for (int i = 0; i < worldSize; i++)
            {
                double d = (noise.GetNoise(i, 0) + 1) / 2;
                heights[i] = (int)(d * worldHeight);
            } 
        }
    }
}
