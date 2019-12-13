using System;
using System.Collections.Generic;
﻿using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Design;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Cyberfuck.Network;
using Cyberfuck.Logger;
using Cyberfuck.Screen;

namespace Cyberfuck
{
	public partial class CyberFuck : Game
	{
		public static SpriteFont font;

		public static Dictionary<string, Texture2D> textures;
		public static INetBase netPlay;
		public static IScreen Screen;
		private static ILogger logger = new ConsoleLogger();

		public static ILogger Logger { get => logger; set => logger = value; }

		public CyberFuck()
		{
			InitializeGraphics();
			Content.RootDirectory = "Content";
		}


		protected override void Initialize()
		{
			base.Initialize();
			Screen = new MainScreen();
			//Host(null);
		}

		public static void Start(string level)
		{
			NetStatus.Type = NetType.Single;
			Screen = new GameScreen();
		}
		public static void Join(string ip, int port)
		{
			NetStatus.Type = NetType.Client;
			netPlay = new NetClient(ip, port);
		}
		public static void Host(string level)
		{
			NetStatus.Type = NetType.Server;
			if (!(Screen is GameScreen))
				Screen = new GameScreen();
			netPlay = new NetServer(1234);
		}
		protected override void Update(GameTime gameTime)
		{
			Input.Update(IsActive);

#if DEBUG
			if(Input.KeyWentDown(Keys.F5))
			{
				if(AssetRebuild.Run())
				{
					UnloadContent();
					LoadContent();
				}
			}
			if(Input.KeyWentDown(Keys.Escape))
			{
				this.Exit();
			}
#endif
			Screen.Update(gameTime);
			base.Update(gameTime);
		}
	}
}