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

namespace Cyberfuck
{
	public partial class CyberFuck : Game
	{
		public static Dictionary<string, Texture2D> textures;
		public static INetBase netPlay;
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
			HostWorld(null);
		}

		public void HostWorld(string level)
		{
			netPlay = new NetServer(1234);
			World.LoadWorld();
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
			World.Update(gameTime);
			netPlay.Update();
			base.Update(gameTime);
		}
	}
}
