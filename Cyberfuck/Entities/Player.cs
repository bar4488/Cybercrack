using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Humper.Responses;
using Cyberfuck.Network;
using Cyberfuck.Data;
using Cyberfuck.Damage;
using Cyberfuck.GameWorld;

namespace Cyberfuck.Entities
{
    public class Player : IFocusable, IEntity
    {
        World world;
        Point velocity;
        List<Shot> shots = new List<Shot>();
        Humper.IBox box;
        const int JUMP_VELOCITY = 24;
        const int MAX_SPEED = 8;
        const int FALL_SPEED = 12;
        const int GRAVITY = 1;
        int jumpCount = 2;
        int id;
        bool directionRight = true;
        bool onGound = false;
        bool midLand = true;
        int fallStart = 0;
        int health = 100;
        bool dead = false;


        PlayerData oldPlayer;

        public int ID => id;
        public Texture2D Texture { get => CyberFuck.textures["player"]; }
        public EntityType Type { get => EntityType.Player; }
        public Point Position { get => new Point((int)box.X, (int)box.Y); }
        public Vector2 VPosition { get => new Vector2((int)box.X, (int)box.Y); }
        public Point Velocity { get => velocity; set => velocity = value; }
        public Vector2 VVelocity { get => new Vector2(velocity.X, velocity.Y); set => velocity = new Point((int)value.X, (int)value.Y); }
        public Rectangle Bounds { get => new Rectangle((int)box.X, (int)box.Y, (int)box.Bounds.Width, (int)box.Bounds.Height); }
        public Vector2 TilePosition => new Vector2(box.X / 16, box.Y / 16);
        public Vector2 TileVelocity => new Vector2(velocity.X / 16, velocity.Y / 16);
        public int Width => Texture.Width;
        public int Height => Texture.Height;
        public int Health => health;

        public Player(World world, int id)
        {
            this.world = world;
            this.id = id;
            Velocity = Point.Zero;
            box = world.CollisionWorld.Create(world.CollisionWorld.Bounds.Width / 2, 0, Texture.Width, Texture.Height);
            box.AddTags(Collider.Player);
        }

        public Player(World world, PlayerData playerData)
        {
            this.world = world;
            this.id = playerData.ID;
            this.Velocity = playerData.Entity.Velocity;
            this.box = world.CollisionWorld.Create(playerData.Entity.Position.X, playerData.Entity.Position.Y, Texture.Width, Texture.Height);
            box.AddTags(Collider.Player);
        }
        public void Draw(GameTime gameTime)
        {
            Vector2 screenSize = new Vector2(CyberFuck.graphics.GraphicsDevice.Viewport.Width, CyberFuck.graphics.GraphicsDevice.Viewport.Height);

            for (int i = 0; i < shots.Count; i++)
            {
                CyberFuck.spriteBatch.Draw(shots[i].Texture, new Vector2(shots[i].Damage.PPosition.X, shots[i].Damage.PPosition.Y), shots[i].Texture.Bounds, Color.White, 0, Vector2.Zero, new Vector2(1f, 1f), SpriteEffects.None, 0f);
            }
            if(!dead)
                CyberFuck.spriteBatch.Draw(Texture, new Vector2(Bounds.X, Bounds.Y), Texture.Bounds, Color.White, 0, Vector2.Zero, new Vector2(1f, 1f), directionRight ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0f);
            else
            {
                if(world.MyPlayerId == ID)
                    CyberFuck.spriteBatch.DrawString(CyberFuck.font, "In my heart you shall forever remain", new Vector2(screenSize.X / 2, 100), Color.Red);
            }
        }

        public void Update(GameTime gameTime)
        {
            foreach (var shot in shots)
            {
                shot.Update(gameTime);
            }
            if (dead)
                return;
            //save the player data in order to compare changes after update
            oldPlayer = new PlayerData(this);
            int velX = Velocity.X;
            int velY = Velocity.Y;
            //move the player to the next position
            var move = box.Move(Position.X + velX, Position.Y + velY, (collision) =>
            {
                if (collision.Other.HasTag(Collider.Tile))
                {
                    return CollisionResponses.Slide;
                }
                if (collision.Other.HasTag(Collider.Player))
                    return CollisionResponses.Ignore;
                return CollisionResponses.Cross;
            });
            //collisions
            IEnumerable<Humper.IHit> TileHits = move.Hits.Where((h) => h.Box.HasTag(Collider.Tile));
            if (TileHits.Any((c) => c.Box.HasTag(Collider.Tile) && (c.Normal.Y == -1 && c.Box.Bounds.Left < Bounds.Right && c.Box.Bounds.Right > Bounds.Left)))
            {
                velY = 0;
                if (!onGound)
                {
                    onGound = true;
                    int deltaY = Position.Y - fallStart;
                    fallStart = Position.Y;
                    if(deltaY > 300)
                    {
                        velY = -4;
                        Damage(30, DamageReason.Fall, "");
                        midLand = true;
                    }
                }
                else if (midLand)
                {
                    midLand = false;
                }
            }
            else if(!midLand)
            {
                onGound = false;
                if (fallStart > Position.Y)
                    fallStart = Position.Y;
            }
            if (TileHits.Any((c) => c.Box.HasTag(Collider.Tile) && (c.Normal.Y == 1 && c.Box.Bounds.Left<Bounds.Right && c.Box.Bounds.Right> Bounds.Left)))
            {
                velY = 0;
            }
            if (TileHits.Any((c) => c.Box.HasTag(Collider.Tile) && (c.Normal.X != 0 && c.Box.Bounds.Top < Bounds.Bottom && c.Box.Bounds.Bottom > Bounds.Top)))
            {
                velX = 0;
            }
            if (TileHits.Any((c) => c.Box.HasTag(Collider.Tile) && (c.Normal.Y < 0 && c.Box.Bounds.Left < Bounds.Right && c.Box.Bounds.Right > Bounds.Left)))
            {
                jumpCount = 3;
                if(ID == world.MyPlayerId)
                {
                    if (!Input.KeyWentDown(Keys.Space) && Input.IsKeyDown(Keys.Space))
                    {
                        if(jumpCount > 0)
                        {
                            jumpCount--;
                            velY = -JUMP_VELOCITY;
                        }
                    }
                }
                if(TileHits.All((c) => c.Box.Bounds.Y >= Bounds.Bottom - Constants.TILE_SIZE) && TileHits.Any((c) => c.Box.Bounds.Y < Bounds.Bottom))
                {
                    velY = -(int)Math.Sqrt(60 * GRAVITY);
                }
            }
            if(velY < FALL_SPEED)
                velY += GRAVITY;

            // input keys
            if(ID == world.MyPlayerId)
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
                        fallStart = Position.Y;
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
                if (Input.LeftMouseWentDown)
                {
                    //shoot

                    Vector2 mousePositionV = Vector2.Transform(new Vector2(Input.MousePosition.X, Input.MousePosition.Y), Matrix.Invert(world.Camera.Transform));
                    Point mousePosition = new Point((int)mousePositionV.X, (int)mousePositionV.Y);
                    Vector2 mouseDirection = (mousePositionV - VPosition);
                    mouseDirection.Normalize();
                    Console.WriteLine(mouseDirection);
                    shots.Add(new Shot(world, VPosition, mouseDirection * 15));
                    directionRight = mouseDirection.X > 0;

                }

                
            }
            if (velX > 0)
                directionRight = true;
            if (velX < 0)
                directionRight = false;

            Velocity = new Point(velX, velY);

            if(oldPlayer != this && (world.NetType == NetType.Server || (world.NetType == NetType.Client && ID == world.MyPlayerId)))
            {
                CyberFuck.netPlay.SendMessage(MessageContentType.PlayerUpdate, ID, new PlayerData(this));
            }
        }

        public void Damage(int amount, Damage.DamageReason reason, string other)
        {
            health -= amount;
            // if server, send new health to all players
            // not needed
            
            if (health < 0)
                Kill();
        }
        public void Kill()
        {
            dead = true;
            // if server send that the player has died
            Thread t = new Thread(() =>
            {
                Thread.Sleep(1000);
                Respawn();
            });
            t.Start();
        }

        public void Respawn()
        {
            dead = false;
            var d = new PlayerData(
                new EntityData(
                    EntityType.Player, 
                    100, 
                    new Point((int)world.CollisionWorld.Bounds.Width / 2, 0), 
                    Point.Zero), 
                ID);
            Apply(d);
        }

        public void Apply(PlayerData toApply)
        {
            if (this.ID != toApply.ID)
                throw new Exception("id doesnt match");
            if(world.NetType == NetType.Server)
                CyberFuck.netPlay.SendMessage(MessageContentType.PlayerUpdate, this.ID, toApply);
            this.box.Move(toApply.Entity.Position.X, toApply.Entity.Position.Y, (c) => CollisionResponses.None);
            this.health = toApply.Entity.Health;
            this.velocity = toApply.Entity.Velocity;
        }

    }
}
