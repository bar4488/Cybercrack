using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Humper;
using Humper.Responses;
using Cyberfuck.Network;
using Cyberfuck.Data;

namespace Cyberfuck.Entities
{
    public class Player: IFocusable, IEntity
    {
        Point velocity;
        IBox box;
        const int JUMP_VELOCITY = 24;
        const int MAX_SPEED = 8;
        const int FALL_SPEED = 12;
        const int GRAVITY = 1;
        int jumpCount = 2;
        int id;
        bool directionRight = true;

        PlayerData oldPlayer;
        PlayerData toApply;

        private Texture2D Texture { get => CyberFuck.textures["player"]; }
        public EntityType Type { get => EntityType.Player; }
        public Point Position { get => new Point((int)box.X, (int)box.Y); }
        public Rectangle Bounds { get => new Rectangle((int)box.X, (int)box.Y, (int)box.Bounds.Width, (int)box.Bounds.Height);  }
        public Point Velocity { get => velocity; set => velocity = value; }

        public int ID => id;

        public Player(int id)
        {
            this.id = id;
            Velocity = Point.Zero;
            box = World.collisionWorld.Create(World.collisionWorld.Bounds.Width/2, 0, Texture.Width, Texture.Height);
        }

        public Player(PlayerData playerData)
        {
            this.id = playerData.ID;
            this.Velocity = playerData.Entity.Velocity;
            this.box = World.collisionWorld.Create(playerData.Entity.Position.X, playerData.Entity.Position.Y, Texture.Width, Texture.Height);
        }
        public void Draw(GameTime gameTime)
        {
            float scale = (float)Math.Sin(gameTime.TotalGameTime.Ticks / 240);
            CyberFuck.spriteBatch.Draw(Texture, new Vector2(Bounds.X, Bounds.Y), Texture.Bounds, Color.White, 0, Vector2.Zero, new Vector2(1f, 1f), directionRight ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0f);
        }

        public void Update(GameTime gameTime)
        {
            oldPlayer = new PlayerData(this);
            int velX = Velocity.X;
            int velY = Velocity.Y;
            if(velY < FALL_SPEED)
                velY += GRAVITY;
            if(ID == World.myPlayerId)
            {
                if (Input.IsKeyDown(Keys.Right) || Input.IsKeyDown(Keys.D))
                {
                    velX = MAX_SPEED;
                }
                else if (Input.IsKeyDown(Keys.Left) || Input.IsKeyDown(Keys.A))
                {
                    velX = -MAX_SPEED;
                }
                else
                    velX = 0;
                if (Input.KeyWentDown(Keys.Space) || Input.KeyWentDown(Keys.Up))
                {
                    if(jumpCount > 0)
                    {
                        jumpCount--;
                        velY = -JUMP_VELOCITY;
                    }
                }

                if (Input.KeyWentDown(Keys.Space) || Input.KeyWentDown(Keys.Up))
                {
                    if (jumpCount > 0)
                    {
                        jumpCount--;
                        velY = -JUMP_VELOCITY;
                    }
                }
                if (Input.KeyWentDown(Keys.Down))
                {
                    velY += 5;
                }
                if (Input.IsKeyDown(Keys.Down))
                {
                    if (velY < FALL_SPEED * 2)
                    {
                        velY += GRAVITY * 2;
                    }
                }
            }
            if (velX > 0)
                directionRight = true;
            if (velX < 0)
                directionRight = false;

            var move = box.Move(Position.X + velX, Position.Y + velY, (collision) =>
            {
                if (collision.Other.HasTag(Collider.Tile))
                {
                    return CollisionResponses.Slide;
                }
                return CollisionResponses.Cross;
            });
            if (move.Hits.Any((c) => c.Box.HasTag(Collider.Tile) && (c.Normal.Y != 0 && c.Box.Bounds.Left < Bounds.Right && c.Box.Bounds.Right > Bounds.Left)))
            {
                velY = 0;
            }
            if (move.Hits.Any((c) => c.Box.HasTag(Collider.Tile) && (c.Normal.X != 0 && c.Box.Bounds.Top < Bounds.Bottom && c.Box.Bounds.Bottom > Bounds.Top)))
            {
                velX = 0;
            }
            if (move.Hits.Any((c) => c.Box.HasTag(Collider.Tile) && (c.Normal.Y < 0 && c.Box.Bounds.Left < Bounds.Right && c.Box.Bounds.Right > Bounds.Left)))
            {
                jumpCount = 3;
                if(ID == World.myPlayerId)
                {
                    if (Input.IsKeyDown(Keys.Space))
                    {
                        if(jumpCount > 0)
                        {
                            jumpCount--;
                            velY = -JUMP_VELOCITY;
                        }
                    }
                }
                IEnumerable<IHit> TileHits = move.Hits.Where((h) => h.Box.HasTag(Collider.Tile));
                if(TileHits.All((c) => c.Box.Bounds.Y >= Bounds.Bottom - Constants.TILE_SIZE) && TileHits.Any((c) => c.Box.Bounds.Y < Bounds.Bottom))
                {
                    velY = -(int)Math.Sqrt(60 * GRAVITY);
                }
            }
            Velocity = new Point(velX, velY);

            if(oldPlayer != this && (NetStatus.Server || (NetStatus.Client && ID == World.myPlayerId)))
            {
                CyberFuck.netPlay.SendMessage(MessageContentType.PlayerUpdate, ID, new PlayerData(this));
            }
        }

        public void Apply(PlayerData toApply)
        {
            if (this.ID != toApply.ID)
                throw new Exception("id doesnt match");
            if(NetStatus.Server)
                CyberFuck.netPlay.SendMessage(MessageContentType.PlayerUpdate, this.ID, toApply);
            this.box.Move(toApply.Entity.Position.X, toApply.Entity.Position.Y, (c) => CollisionResponses.None);
            this.velocity = toApply.Entity.Velocity;
        }

    }
}
