using System;
using System.Collections.Generic;
﻿using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Design;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Cyberfuck
{
	class CyberfuckGame : Game
	{
		GraphicsDeviceManager graphics;
        static CyberfuckGame instance;

        public static CyberfuckGame GetInstance()
        {
            if (instance == null)
                instance = new CyberfuckGame();
            return instance;
        }

		private CyberfuckGame()
		{
			graphics = new GraphicsDeviceManager(this);
			graphics.PreferredBackBufferWidth = 1280;
			graphics.PreferredBackBufferHeight = 720;
			graphics.PreferMultiSampling = true;
			Content.RootDirectory = "Content";

			Window.AllowUserResizing = true;
			IsMouseVisible = true;
		}

		SpriteBatch spriteBatch;
		SpriteFont font;
		Texture2D smile;
		Effect exampleEffect;
        Camera2D camera;

		protected override void Initialize()
		{
			camera = new Camera2D();
			Player player = new Player();
			World world = new World();
			Components.Add(camera);
			camera.Focus = player;
			Components.Add(world);
			Components.Add(player);
			Services.AddService(typeof(ICamera2D), camera);
			base.Initialize();
		}
		protected override void LoadContent()
		{
			spriteBatch = new SpriteBatch(GraphicsDevice);

			Camera2D camera = new Camera2D();
			Services.AddService(typeof(SpriteBatch), spriteBatch);
			exampleEffect = new Effect(GraphicsDevice, File.ReadAllBytes(@"Effects/ExampleEffect.fxb"));

			base.LoadContent();
		}

		protected override void UnloadContent()
		{
			Content.Unload();

			spriteBatch.Dispose();
			exampleEffect.Dispose();

			base.UnloadContent();
		}

		protected override bool BeginDraw()
		{
			spriteBatch.Begin(
				SpriteSortMode.Deferred,
				BlendState.AlphaBlend,
				SamplerState.LinearClamp,
				DepthStencilState.None,
				RasterizerState.CullCounterClockwise,
				null,
				camera.Transform
			);
			GraphicsDevice.Clear(Color.CadetBlue);
			return base.BeginDraw();
		}

		protected override void EndDraw()
		{
			spriteBatch.End();
			base.EndDraw();
		}

		protected override void Update(GameTime gameTime)
		{
			Input.Update(IsActive);

			//
			// Asset Rebuilding:
#if DEBUG
			if(Input.KeyWentDown(Keys.F5))
			{
				if(AssetRebuild.Run())
				{
					UnloadContent();
					LoadContent();
				}
			}
#endif

			//
			// Insert your game update logic here.
			//

			base.Update(gameTime);
		}
	}
}
