using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Humper;

namespace Cyberfuck.GameWorld
{
    public class WorldMap2
    {
        public System.Drawing.Bitmap bitmap;
        public Tile[,] tileMap;
        public IBox[,] collisionMap;
        public Humper.World collisionWorld;
        World world;

        public int Width => tileMap.GetLength(0);
        public int Height => tileMap.GetLength(1);
        public Point Size { get => new Point(Width, Height); }

        public Tile GetBitmapTile(int x, int y)
        {
            switch ((uint)bitmap.GetPixel(x, y).ToArgb())
            {
                case 0xFF000000:
                    return Tile.Dirt;
            }
            return Tile.None;
        }

        public bool AddTile(int x, int y, Tile tile)
        {
            if (x >= tileMap.GetLength(0) || x < 0 || y < 0 || y >= tileMap.GetLength(1))
                return false;
            if(tile == Tile.None)
            {
                if(collisionMap[x, y] != null)
                {
                    collisionWorld.Remove(collisionMap[x, y]);
                    collisionMap[x, y] = null;
                }
            }
            if (tile == Tile.Dirt && tileMap[x, y] == Tile.None)
            {
                var boxes = collisionWorld.Find(x * Constants.TILE_SIZE, y * Constants.TILE_SIZE, Constants.TILE_SIZE, Constants.TILE_SIZE);
                foreach (var box in boxes)
                {
                    if (box.HasTag(Collider.Player))
                        if(box.Bounds.Intersects(new Humper.Base.RectangleF(x * Constants.TILE_SIZE, y * Constants.TILE_SIZE, Constants.TILE_SIZE, Constants.TILE_SIZE)))
                            return false;
                }
                collisionMap[x, y] = collisionWorld.Create(x * Constants.TILE_SIZE, y * Constants.TILE_SIZE, Constants.TILE_SIZE, Constants.TILE_SIZE).AddTags(Collider.Tile);
            }
            tileMap[x, y] = tile;
            return true;
        }

        public WorldMap2(World world, int seed) : this(world, MapFromSeed(seed)) { }

        private static Tile[,] MapFromSeed(int seed)
        {
            Tile[,] map = new Tile[WorldGen.worldSize, WorldGen.worldHeight];
            WorldGen g = new WorldGen();
            for (int i = 0; i < map.GetLength(0); i++)
            {
                int height = g.heights[i];
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    map[i, j] = height > map.GetLength(1) - j ? Tile.Dirt : Tile.None;
                }
            }
            return map;
        }
        public WorldMap2(World world, Tile[,] map)
        {
            this.world = world;
            tileMap = new Tile[map.GetLength(0), map.GetLength(1)];
            collisionMap = new IBox[map.GetLength(0), map.GetLength(1)];
            collisionWorld = new Humper.World(map.GetLength(0) * Constants.TILE_SIZE, map.GetLength(1) * Constants.TILE_SIZE);
            for(int x = 0; x < map.GetLength(0); x++)
            {
                for(int y = 0; y <map.GetLength(1); y++)
                {
                    AddTile(x, y, map[x, y]);
                }
            }
        }
        public WorldMap2(World world, System.Drawing.Bitmap map)
        {
            this.world = world;
            bitmap = map;
            tileMap = new Tile[map.Width, map.Height];
            collisionMap = new IBox[map.Width, map.Height];
            collisionWorld = new Humper.World(map.Width * Constants.TILE_SIZE, map.Height * Constants.TILE_SIZE);
            
            for(int x = 0; x < Width; x++)
            {
                for(int y = 0; y <Height; y++)
                {
                    AddTile(x, y, GetBitmapTile(x, y));
                }
            }
        }
        public WorldMap2(World world, string level = "Level1.png"): this(world, new System.Drawing.Bitmap(@"Content/Levels/" + level))
        {
        }
        public void Draw(GameTime gameTime)
        {
            int width = CyberFuck.graphics.GraphicsDevice.Viewport.Width;
            int height = CyberFuck.graphics.GraphicsDevice.Viewport.Height;

            Point cameraPosition = world.Camera.Position;
            int startTileX = (int)((cameraPosition.X - width / 2) / 16 - 1);
            int startTileY = (int)((cameraPosition.Y - height / 2) / 16 - 1);
            int endTileX = (int)((cameraPosition.X + width / 2) / 16 + 2);
            int endTileY = (int)((cameraPosition.Y + height / 2) / 16 + 2);

            if (startTileX < 0)
                startTileX = 0;
            if (startTileY < 0)
                startTileY = 0;
            if (endTileX > Width)
                endTileX = Width;
            if (endTileY > Height)
                endTileY = Height;

            for(int x = startTileX; x < endTileX; x++)
            {
                for(int y = startTileY; y < endTileY; y++)
                {
                    if (tileMap[x, y] == Tile.Dirt)
                        CyberFuck.spriteBatch.Draw(CyberFuck.GetTexture("tileDirt") , new Rectangle(Constants.TILE_SIZE * x, Constants.TILE_SIZE * y, Constants.TILE_SIZE, Constants.TILE_SIZE), new Rectangle(0, 0, CyberFuck.GetTexture("tileDirt").Width, CyberFuck.GetTexture("tileDirt").Height), Color.White);
                }
            }
        }
    }
}
