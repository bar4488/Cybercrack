using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cyberfuck.GameWorld;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Cyberfuck.GameObjects.Items
{
    class Placorator: IItem
    {
        public int ItemID { get => 2; }
        public int OwnerID { get; set; }
        public bool IsOwned { get; set; }
        public IEntity Holder { get; set; }
        public Texture2D Texture => CyberFuck.GetTexture("placorator");

        World world;

        public Placorator(World world, IEntity entity)
        {
            this.world = world;
            world.GameObjects.Add(this);
            Holder = entity;
        }

        public void Use(GameTime gameTime, Vector2 MousePosition)
        {
            Vector2 mousePositionV = Vector2.Transform(new Vector2(Input.MousePosition.X, Input.MousePosition.Y), Matrix.Invert(world.Camera.Transform));
            world.AddTile((int)(mousePositionV.X/16), (int)(mousePositionV.Y/16), Tile.Dirt);
        }
        public void SecondUse(GameTime gameTime, Vector2 MousePosition)
        {
            Vector2 mousePositionV = Vector2.Transform(new Vector2(Input.MousePosition.X, Input.MousePosition.Y), Matrix.Invert(world.Camera.Transform));
            world.AddTile((int)(mousePositionV.X/16), (int)(mousePositionV.Y/16), Tile.None);
        }

        public void Update(GameTime gameTime)
        {
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.Draw(CyberFuck.GetTexture("placorator"), new Rectangle(Holder.Position.X, Holder.Position.Y, Texture.Width, Texture.Height), Color.Transparent);
        }
    }
}
