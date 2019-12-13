using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Cyberfuck.Entities;
using Cyberfuck.Data;
using Cyberfuck.Network;

namespace Cyberfuck
{
    class World
    {
        public static Humper.World collisionWorld;
        public static WorldMap map;
        public static Camera2D camera;
        public static int myPlayerId;
        public static Player player { get => players[myPlayerId]; }
        public static Dictionary<int, Player> players = new Dictionary<int, Player>();
        public static List<IEntity> entities = new List<IEntity>();

        public static void LoadWorld(string level = "Level3.png")
        {
            players.Clear();
            entities.Clear();
            map = new WorldMap(level);
            collisionWorld = map.world;
            if(Network.NetStatus.Single || Network.NetStatus.Server)
            {
                myPlayerId = -1;
                players[-1] = new Player(myPlayerId);
            }
            camera = new Camera2D();
            camera.Focus = players[myPlayerId];
        }
        public static void LoadWorld(System.Drawing.Bitmap level)
        {
            players.Clear();
            entities.Clear();
            map = new WorldMap(level);
            collisionWorld = map.world;
            if(Network.NetStatus.Single || Network.NetStatus.Server)
            {
                myPlayerId = -1;
                players[-1] = new Player(myPlayerId);
                camera = new Camera2D();
                camera.Focus = players[myPlayerId];
            }
        }

        public static void LoadEntities(List<EntityData> entities)
        {
        }

        public static void LoadPlayers(List<PlayerData> playersData)
        {
            foreach (var playerData in playersData)
            {
                LoadPlayer(playerData, false);
            }
        }
        public static void LoadPlayer(PlayerData playerData, bool local)
        {
            lock (players)
            {
                players[playerData.ID] = new Player(playerData);
                if (local)
                {
                    myPlayerId = playerData.ID;
                    camera = new Camera2D();
                    camera.Focus = players[myPlayerId];
                }
            }
        }
        public static void RemovePlayer(int id)
        {
            lock (players)
            {
                players.Remove(id);
            }
        }
        public static void Update(GameTime gameTime)
        {
            if(!NetStatus.Single)
                CyberFuck.netPlay.SnapShot();
            lock (players)
            {
                foreach (var player in players.Values.ToArray())
                {
                    player.Update();
                }
            }
            camera.Update(gameTime);
            if(!NetStatus.Single)
                CyberFuck.netPlay.Update();
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
            lock (players)
            {
                foreach (var player in players.Values.ToArray())
                {
                    player.Draw();
                }
            }
            spriteBatch.End();
        }

    }
}
