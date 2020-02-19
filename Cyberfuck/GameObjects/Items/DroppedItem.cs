using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cyberfuck.GameObjects;
using Cyberfuck.GameWorld;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Humper.Responses;
using System.Threading;

namespace Cyberfuck.GameObjects.Items
{
    public class DroppedItem : IGameObject
    {
        protected World world;
        public IItem Item;
        protected Humper.IBox box;
        public Vector2 Position => new Vector2(box.X, box.Y);
        protected Vector2 Velocity { get; set; }

        public DroppedItem(World world, Vector2 position, IItem item)
        {
            this.world = world;
            this.Item = item;
            this.Velocity = new Vector2(0, -10);
            box = world.CollisionWorld.Create(position.X, position.Y, item.Texture.Width, item.Texture.Height);
            Thread t = new Thread(() =>
            {
                Thread.Sleep(2000);
                box.AddTags(Collider.Item);
                box.Data = this;
            });
            t.Start();
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.Draw(Item.Texture, new Rectangle((int)Position.X, (int)Position.Y, Item.Texture.Width, Item.Texture.Height), Color.White);
        }

        public void Update(GameTime gameTime)
        {
            bool collidedTile = false;
            var move = box.Move(Position.X + Velocity.X, Position.Y + Velocity.Y, (col) =>
            {
                if (col.Other.HasTag(Collider.Tile))
                {
                    collidedTile = true;
                    return CollisionResponses.Bounce;
                }
                return CollisionResponses.Ignore;
            });
            if (box.HasTag(Collider.Item))
            {
                // check if any players are near
                int offset = Constants.TILE_SIZE * 3;
                var boxes = world.CollisionWorld.Find(new Humper.Base.RectangleF(Position.X - offset, Position.Y - offset, Item.Texture.Width + offset, Item.Texture.Height + offset));
                var players = boxes.Where((b) => b.HasTag(Collider.Player));
                if (players.Any())
                {
                    Humper.IBox player = players.First();
                    Vector2 lDist = new Vector2(player.X, player.Y) - Position;
                    foreach (var p in players.ToArray())
                    {
                        var dist = new Vector2(p.X, p.Y) - Position;
                        if(lDist.LengthSquared() > dist.LengthSquared())
                        {
                            lDist = dist;
                            player = p;
                        }
                    }
                    var direction = Vector2.Normalize(lDist) * 3;
                    box.Move(box.X + direction.X, box.Y + direction.Y, (col) => CollisionResponses.Ignore);
                }
            }
            Vector2 vel = Vector2.Zero;
            if (collidedTile)
                vel = new Vector2(move.Destination.X - move.Origin.X, move.Destination.Y - move.Origin.Y);
            else
                vel = new Vector2(Velocity.X, Velocity.Y + 1);
            Velocity = 0.9f * vel;
        }
        public void Remove()
        {
            lock (world.GameObjects)
            {
                world.CollisionWorld.Remove(box);
                world.GameObjects.Remove(this);
            }
        }
    }
}
