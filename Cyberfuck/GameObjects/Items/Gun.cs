using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cyberfuck.GameObjects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Cyberfuck.Data;
using Cyberfuck.GameWorld;

namespace Cyberfuck.GameObjects.Items
{
    class Gun: IItem
    {
        const int SHOT_SPEED = 15;

        public int ItemID { get => 1; }
        public int OwnerID { get; set; }
        public bool IsOwned { get; set; }
        public IEntity Holder { get; set; }
        public Texture2D Texture => CyberFuck.GetTexture("gun");

        List<PVDataF> shots = new List<PVDataF>();
        World world;

        public Gun(World world, IEntity entity)
        {
            this.world = world;
            world.GameObjects.Add(this);
            Holder = entity;
        }

        public void Use(GameTime gameTime, Vector2 mousePosition)
        {
            Vector2 mouseDirection = (mousePosition - new Vector2(Holder.Position.X, Holder.Position.Y));
            mouseDirection.Normalize();
            shots.Add(new PVDataF(new Vector2(Holder.Position.X, Holder.Position.Y), Vector2.Normalize(mouseDirection) * SHOT_SPEED));
        }

        public void SecondUse(GameTime gameTime, Vector2 MousePosition)
        {

        }

        public void Update(GameTime gameTime)
        {
            for (int i = 0; i < shots.Count; i++)
            {
                PVDataF shot = shots[i];
                shot.Position = shot.Position + shot.Velocity;
                shots[i] = shot;
            }
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            foreach (var shot in shots)
            {
                spriteBatch.Draw(CyberFuck.GetTexture("shot"), shot.Position, Color.White);
            }
        }
    }
}
