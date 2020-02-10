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

        public void Use(GameTime gameTime, Vector2 mousePosition)
        {
            if(world.NetType != Network.NetType.Client)
            {
                Vector2 mouseDirection = mousePosition - new Vector2(Holder.Position.X, Holder.Position.Y);
                if(mouseDirection.Length() < 5*16)
                    world.AddTile((int)(mousePosition.X/16), (int)(mousePosition.Y/16), Tile.Dirt);
            }
        }
        public void SecondUse(GameTime gameTime, Vector2 mousePosition)
        {
            if(world.NetType != Network.NetType.Client)
                world.AddTile((int)(mousePosition.X/16), (int)(mousePosition.Y/16), Tile.None);
        }

        public void Update(GameTime gameTime)
        {
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
        }
    }
}
