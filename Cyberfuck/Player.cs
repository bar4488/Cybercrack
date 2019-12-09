using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace Cyberfuck
{
    class Player: DrawableGameComponent, IFocusable
    {
        Point position;
        Texture2D texture;

        public Player(Game game) : base(game) { }
        public Point Position { get => position; set => position = value; }
        public Rectangle Bounds { get => new Rectangle(Position.X, Position.Y, texture.Width, texture.Height);  }

        public override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            texture = Game.Content.Load<Texture2D>("Gore_1005");
        }
        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = Game.Services.GetService(typeof(SpriteBatch)) as SpriteBatch;
            spriteBatch.Draw(texture, Bounds, texture.Bounds, Color.White);
        }

        public override void Update(GameTime gameTime)
        {
            int x = Position.X;
            int y = Position.Y;
            if (Input.IsKeyDown(Keys.Right))
                x += 5;
            if (Input.IsKeyDown(Keys.Left))
                x += -5;
            if (Input.IsKeyDown(Keys.Up))
                y += -5;
            if (Input.IsKeyDown(Keys.Down))
                y += 5;
            ICamera2D camera = Game.Services.GetService(typeof(ICamera2D)) as ICamera2D;
            Position = new Point(x, y);
        }
    }
}
