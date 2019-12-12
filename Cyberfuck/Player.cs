using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Humper;
using Humper.Responses;

namespace Cyberfuck
{
    public class Player: IFocusable, IEntity
    {
        Point velocity;
        IBox box;
        const int JUMP_VELOCITY = 24;
        const int MAX_SPEED = 8;
        const int FALL_SPEED = 12;
        const int gravity = 1;
        int jumpCount = 2;
        int id;

        PlayerData oldPlayer;

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
        public void Draw()
        {
            CyberFuck.spriteBatch.Draw(Texture, Bounds, Texture.Bounds, Color.White);
        }

        public void Update()
        {
            oldPlayer = new PlayerData(this);
            int velX = Velocity.X;
            int velY = Velocity.Y;
            if(velY < FALL_SPEED)
                velY += gravity;
            if (Input.IsKeyDown(Keys.Right))
                velX = MAX_SPEED;
            else if (Input.IsKeyDown(Keys.Left))
                velX = -MAX_SPEED;
            else
                velX = 0;
            if (Input.KeyWentDown(Keys.Space))
            {
                if(jumpCount > 0)
                {
                    jumpCount--;
                    velY = -JUMP_VELOCITY;
                }
            }

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
                jumpCount = 0;
                if (Input.IsKeyDown(Keys.Space))
                {
                    velY = -JUMP_VELOCITY;
                    jumpCount = 2;
                }
                if(move.Hits.All((c) => c.Box.Bounds.Y >= Bounds.Bottom - Constants.TILE_SIZE) && move.Hits.Any((c) => c.Box.Bounds.Y < Bounds.Bottom))
                {
                    velY = -(int)Math.Sqrt(60 * gravity);
                }
            }
            Velocity = new Point(velX, velY);

            if(oldPlayer != this)
            {
                CyberFuck.netPlay.SendMessage(Network.MessageType.PlayerUpdate, World.playerId, new PlayerData(this));
            }
        }

    }
}
