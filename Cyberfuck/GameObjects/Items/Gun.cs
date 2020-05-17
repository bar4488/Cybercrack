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
        protected const int SHOT_SPEED = 15;
        protected const int damageAmount = 15;

        public virtual int ItemID { get => 1; }
        public int OwnerID { get; set; }
        public bool IsOwned { get; set; }
        public IEntity Holder { get; set; }
        public virtual Texture2D Texture => CyberFuck.GetTexture("gun");

        public virtual Texture2D ShotTexture => CyberFuck.GetTexture("shot");

        protected List<Tuple<double, VelocityObject>> shots = new List<Tuple<double, VelocityObject>>();
        protected World world;

        public Gun(World world, IEntity entity)
        {
            this.world = world;
            world.GameObjects.Add(this);
            Holder = entity;
        }

        public void Use(GameTime gameTime, Vector2 mousePosition)
        {
            Vector2 mouseDirection = mousePosition - new Vector2(Holder.Position.X, Holder.Position.Y);
            mouseDirection.Normalize();
            Humper.IBox box = world.CollisionWorld.Create(Holder.Position.X+Holder.Width/2, Holder.Position.Y+Holder.Height/2, ShotTexture.Width, ShotTexture.Height);
            shots.Add(new Tuple<double, VelocityObject>(gameTime.TotalGameTime.TotalMilliseconds, new VelocityObject(box, Vector2.Normalize(mouseDirection) * SHOT_SPEED)));
        }

        public void SecondUse(GameTime gameTime, Vector2 MousePosition)
        {

        }

        public void Update(GameTime gameTime)
        {
            foreach (var shotK in shots.ToArray())
            {
                VelocityObject shot = shotK.Item2;
                if (MoveShot(shot))
                {
                    world.CollisionWorld.Remove(shot.Box);
                    shots.Remove(shotK);
                }
            }
            while (shots.Count > 0)
            {
                if (gameTime.TotalGameTime.TotalMilliseconds - shots[0].Item1 > 5000)
                {
                    world.CollisionWorld.Remove(shots[0].Item2.Box);
                    shots.RemoveAt(0);
                }
                else
                    break;
            }
        }

        // returns if shot should be removed
        public virtual bool MoveShot(VelocityObject shot)
        {
            bool shouldRemove = false;
            shot.Box.Move(shot.Position.X + shot.Velocity.X, shot.Position.Y + shot.Velocity.Y, (col) => {
                if (col.Other.HasTags(Collider.Tile))
                {
                    shouldRemove = true;
                }
                if (col.Other.HasTags(Collider.Player))
                {
                    if(Holder.ID != (int)col.Other.Data)
                    {
                        world.Players[(int)col.Other.Data].Damage(damageAmount, DamageReason.Enemy, "a gun shot you", Holder.ID);
                        shouldRemove = true;
                    }
                }
                return new Humper.Responses.IgnoreResponse(col);
            });
            return shouldRemove;
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            foreach (var shot in shots)
            {
                spriteBatch.Draw(ShotTexture, shot.Item2.Position, Color.White);
            }
        }
    }
}
