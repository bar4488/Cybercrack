using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Cyberfuck
{
    public enum Tile
    {
        Dirt,
        Grass,
        None,
    }
    public class WorldMap
    {
        public System.Drawing.Bitmap bitmap;
        public Tile[,] tileMap;
        public Humper.World world;

        public int Width => tileMap.GetLength(0);
        public int Height => tileMap.GetLength(1);
        public Point Size { get => new Point(Width, Height); }

        public Tile GetTile(int x, int y)
        {
            switch ((uint)bitmap.GetPixel(x, y).ToArgb())
            {
                case 0xFF000000:
                    return Tile.Dirt;
            }
            return Tile.None;
        }

        public WorldMap(System.Drawing.Bitmap map)
        {
            bitmap = map;
            tileMap = new Tile[map.Width, map.Height];
            world = new Humper.World(map.Width * Constants.TILE_SIZE, map.Height * Constants.TILE_SIZE);
            
            for(int x = 0; x < Width; x++)
            {
                for(int y = 0; y <Height; y++)
                {
                    tileMap[x, y] = GetTile(x, y);
                    if (tileMap[x,y] == Tile.Dirt)
                    {
                        world.Create(x * Constants.TILE_SIZE, y * Constants.TILE_SIZE, Constants.TILE_SIZE, Constants.TILE_SIZE).AddTags(Collider.Tile);
                    }

                }
            }
        }
        public WorldMap(string level = "Level1.png"): this(new System.Drawing.Bitmap(@"Content/Levels/" + level))
        {
        }
        public void Draw(GameTime gameTime)
        {
            int width = CyberFuck.graphics.GraphicsDevice.Viewport.Width;
            int height = CyberFuck.graphics.GraphicsDevice.Viewport.Height;

            Point cameraPosition = World.camera.Position;
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
                        CyberFuck.spriteBatch.Draw(CyberFuck.textures["tileDirt"] , new Rectangle(Constants.TILE_SIZE * x, Constants.TILE_SIZE * y, Constants.TILE_SIZE, Constants.TILE_SIZE), new Rectangle(0, 0, CyberFuck.textures["tileDirt"].Width, CyberFuck.textures["tileDirt"].Height), Color.White);
                }
            }
        }
    }
}
