using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace Cyberfuck
{
    public partial class CyberFuck
    {
		public static GraphicsDeviceManager graphics;
		public static SpriteBatch spriteBatch;
		static ContentManager content;

        public void InitializeGraphics()
        {
			graphics = new GraphicsDeviceManager(this);
			graphics.PreferredBackBufferWidth = 1280;
			graphics.PreferredBackBufferHeight = 720;
			graphics.PreferMultiSampling = true;
			Window.AllowUserResizing = true;
			IsMouseVisible = true;
			Textures = new Dictionary<string, Texture2D>();
        }

		protected override void LoadContent()
		{
			content = Content;
			spriteBatch = new SpriteBatch(GraphicsDevice);
            GetTexture("player");
			GetTexture("tileDirt");
			GetTexture("background");
			GetTexture("shot");
			GetTexture("rect");
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

		public static Texture2D GetTexture(string texture)
		{
			if (!Textures.ContainsKey(texture))
                Textures.Add(texture, content.Load<Texture2D>(texture));
            return Textures[texture];
		}
    }
}
