using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Cyberfuck
{
    class World
    {
        public static Humper.World collisionWorld;
        public static WorldMap map;
        public static Camera2D camera;
        public static int playerId;
        public static Player player;
        public static Dictionary<int, Player> players = new Dictionary<int, Player>();
        public static List<IEntity> entities = new List<IEntity>();

        public static void LoadWorld(string level = "Level3.png")
        {
            players.Clear();
            entities.Clear();
            map = new WorldMap(level);
            collisionWorld = map.world;
            if(Network.NetStatus.netType == Network.NetType.Single || Network.NetStatus.netType == Network.NetType.Server)
            {
                player = new Player(0);
                playerId = 0;
                players[0] = player;
            }
            camera = new Camera2D();
            camera.Focus = player;

        }
        public static void Update(GameTime gameTime)
        {
            player.Update();
            camera.Update(gameTime);
        }

        public static void Draw(SpriteBatch spriteBatch)
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
            map.Draw();
            player.Draw();
            spriteBatch.End();
        }

    }
}
