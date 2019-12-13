using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Cyberfuck
{
    public partial class CyberFuck
    {
		public static GraphicsDeviceManager graphics;
		public static SpriteBatch spriteBatch;

        public void InitializeGraphics()
        {
			graphics = new GraphicsDeviceManager(this);
			graphics.PreferredBackBufferWidth = 1280;
			graphics.PreferredBackBufferHeight = 720;
			graphics.PreferMultiSampling = true;
			Window.AllowUserResizing = true;
			IsMouseVisible = true;
			textures = new Dictionary<string, Texture2D>();
        }

		protected override void LoadContent()
		{
			spriteBatch = new SpriteBatch(GraphicsDevice);
            textures.Add("player", Content.Load<Texture2D>("Gore_1005"));
			textures.Add("tileDirt", Content.Load<Texture2D>(@"Dirt"));
			font = Content.Load<SpriteFont>("Font");
			base.LoadContent();
		}

		protected override void UnloadContent()
		{
			Content.Unload();

			spriteBatch.Dispose();

			base.UnloadContent();
		}

		protected override bool BeginDraw()
		{
			GraphicsDevice.Clear(Color.CadetBlue);
			return base.BeginDraw();
		}

		protected override void Draw(GameTime gameTime)
		{
			Screen.Draw(gameTime, spriteBatch);
		}

		protected override void EndDraw()
		{
			base.EndDraw();
		}

    }
}
