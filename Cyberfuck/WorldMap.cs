using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Humper;

namespace Cyberfuck
{
    enum Collider
    {
        Tile,
        Enemy,

    }
    enum Tile
    {
        Dirt,
        Grass,
        None,
    }
    class WorldMap: DrawableGameObject
    {
        System.Drawing.Bitmap worldMap;
        Texture2D smiley;
        World world;
        public Point Size { get => new Point(worldMap.Width, worldMap.Height); }

        public Tile GetTile(int x, int y)
        {
            switch ((uint)worldMap.GetPixel(x, y).ToArgb())
            {
                case 0xFF000000:
                    return Tile.Dirt;
            }
            return Tile.None;
        }

        public WorldMap()
        {
            this.worldMap = new System.Drawing.Bitmap(@"Content/Level1.png");
            world = new World(worldMap.Width * 16, worldMap.Height * 16);
            Game.Services.AddService(typeof(World), world);
            for(int x = 0; x < worldMap.Width; x++)
            {
                for(int y = 0; y < worldMap.Height; y++)
                {
                    if ((uint)worldMap.GetPixel(x, y).ToArgb() == 0xFF000000)
                    {
                        world.Create(x * 16, y * 16, 16, 16).AddTags(Collider.Tile);
                    }

                }
            }

        }

        protected override void LoadContent()
        {
			smiley = Game.Content.Load<Texture2D>(@"Dirt");
        }
        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = Game.Services.GetService(typeof(SpriteBatch)) as SpriteBatch;
            for(int x = 0; x < worldMap.Width; x++)
            {
                for(int y = 0; y < worldMap.Height; y++)
                {
                    if (GetTile(x,y) == Tile.Dirt)
                        spriteBatch.Draw(smiley, new Rectangle(16 * x, 16 * y, 16, 16), new Rectangle(0, 0, smiley.Width, smiley.Height), Color.White);
                }
            }
        }
    }
}
