using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Cyberfuck
{
    enum Collider
    {
        Tile,
        Enemy,

    }
    public enum TileType
    {
        Dirt,
        Grass,
        None,
    }
    public class WorldMap
    {
        public System.Drawing.Bitmap bitmap;
        public Humper.World world;
        public Point Size { get => new Point(bitmap.Width, bitmap.Height); }

        public TileType GetTile(int x, int y)
        {
            switch ((uint)bitmap.GetPixel(x, y).ToArgb())
            {
                case 0xFF000000:
                    return TileType.Dirt;
            }
            return TileType.None;
        }

        public WorldMap(string level = "Level1.png")
        {
            this.bitmap = new System.Drawing.Bitmap(@"Content/" + level);
            world = new Humper.World(bitmap.Width * Constants.TILE_SIZE, bitmap.Height * Constants.TILE_SIZE);
            
            for(int x = 0; x < bitmap.Width; x++)
            {
                for(int y = 0; y < bitmap.Height; y++)
                {
                    if (GetTile(x, y) == TileType.Dirt)
                    {
                        world.Create(x * Constants.TILE_SIZE, y * Constants.TILE_SIZE, Constants.TILE_SIZE, Constants.TILE_SIZE).AddTags(Collider.Tile);
                    }

                }
            }

        }
        public void Draw()
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
            if (endTileX > bitmap.Width)
                endTileX = bitmap.Width;
            if (endTileY > bitmap.Height)
                endTileY = bitmap.Height;

            for(int x = startTileX; x < endTileX; x++)
            {
                for(int y = startTileY; y < endTileY; y++)
                {
                    if (GetTile(x,y) == TileType.Dirt)
                        CyberFuck.spriteBatch.Draw(CyberFuck.textures["tileDirt"] , new Rectangle(Constants.TILE_SIZE * x, Constants.TILE_SIZE * y, Constants.TILE_SIZE, Constants.TILE_SIZE), new Rectangle(0, 0, CyberFuck.textures["tileDirt"].Width, CyberFuck.textures["tileDirt"].Height), Color.White);
                }
            }
        }
    }
}
