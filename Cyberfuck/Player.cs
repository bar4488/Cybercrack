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
    class Player: DrawableGameObject, IFocusable
    {
        Vector2 velocity;
        Texture2D texture;
        IBox box;
        const int JUMP_VELOCITY = 24;
        const int MAX_SPEED = 8;
        const int FALL_SPEED = 12;
        const int gravity = 1;
        int jumpCount = 2;

        public Point Position { get => new Point((int)box.X, (int)box.Y); }
        public Vector2 PositionV { get => new Vector2(box.X, box.Y); }
        public Rectangle Bounds { get => new Rectangle((int)box.X, (int)box.Y, (int)box.Bounds.Width, (int)box.Bounds.Height);  }
        public Vector2 Velocity { get => velocity; set => velocity = value; }

        public override void Initialize()
        {
            base.Initialize();
            Velocity = Vector2.Zero;
        }

        protected override void LoadContent()
        {
            texture = Game.Content.Load<Texture2D>("Gore_1005");
            World world = Game.Services.GetService(typeof(World)) as World;
            box = world.Create(world.Bounds.Width/2, 0, texture.Width, texture.Height);
        }
        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = Game.Services.GetService(typeof(SpriteBatch)) as SpriteBatch;
            spriteBatch.Draw(texture, Bounds, texture.Bounds, Color.White);
        }

        public override void Update(GameTime gameTime)
        {
            float velX = Velocity.X;
            float velY = Velocity.Y;
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
                    jumpCount--;
                    velY = -JUMP_VELOCITY;
            }

            var move = box.Move(Position.X + velX, Position.Y + velY, (collision) =>
            {
                Console.WriteLine(collision.Goal);
                Console.WriteLine(Position);
                Console.WriteLine("V: {0}, {1}", velX, velY);
                if (collision.Other.HasTag(Collider.Tile))
                {
                    return CollisionResponses.Slide;
                }
                return CollisionResponses.Cross;
            });
            if(move.HasCollided)
                Console.WriteLine(Position);
            if (move.Hits.Any((c) => c.Box.HasTag(Collider.Tile) && (c.Normal.Y != 0 && c.Box.Bounds.Left < Bounds.Right && c.Box.Bounds.Right > Bounds.Left)))
            {
                velY = 0;
            }
            if (move.Hits.Any((c) => c.Box.HasTag(Collider.Tile) && (c.Normal.X != 0 && c.Box.Bounds.Top < Bounds.Bottom && c.Box.Bounds.Bottom > Bounds.Top)))
            {
                velX = 0;
            }
            /*
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
                    velY = -(float)Math.Sqrt(40 * gravity);
                }
            }
            */
            Velocity = new Vector2(velX, velY);
        }
    }
}
