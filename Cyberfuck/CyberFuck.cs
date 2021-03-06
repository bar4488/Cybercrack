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
    enum Collider
    {
        Tile = 1 << 0,
        Enemy = 1 << 1,
        Player = 1 << 2,
		// a tile which the user can step on
        Slope = 1 << 3,
		Damage = 1<<4,
		Item = 1<<5,
    }
	public enum DamageReason
	{
		Fall,
		Enemy,
		Player
	}
	public partial class CyberFuck : Game
	{
		public static SpriteFont font;
		private static Dictionary<string, Texture2D> Textures;
		public static INetBase netPlay;
		public static IScreen Screen;
		private static ILogger logger = new ConsoleLogger();
		public static CyberFuck instance;

		public static ILogger Logger { get => logger; set => logger = value; }

		public CyberFuck()
		{
			instance = this;
			InitializeGraphics();
			Content.RootDirectory = "Content";
		}

		protected override void Initialize()
		{
			base.Initialize();
			Screen = new MainScreen();
			//Host(null);
		}

		public static void Start(string level, string name)
		{
            GameWorld.World w = new GameWorld.World(name);
            if (level == "test")
                w.LoadWorld(new GameWorld.WorldMap(w, 0));
            else
                w.LoadWorld(level);
			Screen = new GameScreen(w);
			((GameScreen)Screen).World.NetType = NetType.Single;
		}
		public static void Join(string ip, int port, string name)
		{
			netPlay = new NetClient(ip, port, name);
            netPlay.OnClose += (r) =>
            {
                Screen = new MainScreen();
            };
            ((NetClient)netPlay).OnConnected += world =>
            {
                Screen = new GameScreen(world);
            };
		}

        public static void Host(string level, string name)
		{
			if (!(Screen is GameScreen))
            {
                GameWorld.World w = new GameWorld.World(name);
                if (level == "test")
                    w.LoadWorld(new GameWorld.WorldMap(w, 0));
                else
                    w.LoadWorld(level);
				Screen = new GameScreen(level, name);
            }
			((GameScreen)Screen).World.NetType = NetType.Server;
			netPlay = new NetServer(((GameScreen)Screen).World, 1234);
		}
		protected override void Update(GameTime gameTime)
		{
			Input.Update(IsActive);
			if(Input.KeyWentDown(Keys.R))
			{
				ReloadContent();
			}
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
			Screen.Update(gameTime);
			base.Update(gameTime);
		}
	}
}
