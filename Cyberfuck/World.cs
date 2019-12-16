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
    public class World
    {
        private static Humper.World collisionWorld;
        private static WorldMap map;
        private static Camera2D camera;
        private static int myPlayerId;
        private static Dictionary<int, Player> players = new Dictionary<int, Player>();
        private static List<IEntity> entities = new List<IEntity>();

        public Player Player { get => players[myPlayerId]; }
        public WorldMap Map { get => map; set => map = value; }
        public Camera2D Camera { get => camera; set => camera = value; }
        public int MyPlayerId { get => myPlayerId; set => myPlayerId = value; }
        public Dictionary<int, Player> Players { get => players; set => players = value; }
        public List<IEntity> Entities { get => entities; set => entities = value; }
        public Humper.World CollisionWorld { get => collisionWorld; set => collisionWorld = value; }

        public World()
        {

        }
        public void LoadWorld(string level = "Level3.png")
        {
            World world = new World();
            world.Players.Clear();
            world.Entities.Clear();
            world.Map = new WorldMap(world, level);
            world.CollisionWorld = map.collisionWorld;
            if(Network.NetStatus.Single || Network.NetStatus.Server)
            {
                world.MyPlayerId = -1;
                world.Players[-1] = new Player(world, myPlayerId);
            }
            world.Camera = new Camera2D();
            world.Camera.Focus = players[myPlayerId];
        }
        public void LoadWorld(System.Drawing.Bitmap level)
        {
            World world = new World();
            players.Clear();
            entities.Clear();
            map = new WorldMap(world, level);
            collisionWorld = map.collisionWorld;
            if(Network.NetStatus.Single || Network.NetStatus.Server)
            {
                myPlayerId = -1;
                players[-1] = new Player(world, myPlayerId);
                camera = new Camera2D();
                camera.Focus = players[myPlayerId];
            }
        }

        public void LoadEntities(List<EntityData> entities)
        {
            entities.AddRange(entities);
        }

        public void LoadPlayers(List<PlayerData> playersData)
        {
            foreach (var playerData in playersData)
            {
                LoadPlayer(playerData, false);
            }
        }
        public void LoadPlayer(PlayerData playerData, bool local)
        {
            lock (players)
            {
                players[playerData.ID] = new Player(this, playerData);
                if (local)
                {
                    myPlayerId = playerData.ID;
                    camera = new Camera2D();
                    camera.Focus = players[myPlayerId];
                }
            }
        }
        public void RemovePlayer(int id)
        {
            lock (players)
            {
                players.Remove(id);
            }
        }
        public void Update(GameTime gameTime)
        {
            if(!NetStatus.Single)
                CyberFuck.netPlay.SnapShot();
            lock (players)
            {
                foreach (var player in players.Values.ToArray())
                {
                    player.Update(gameTime);
                }
            }
            camera.Update(gameTime);
            if(!NetStatus.Single)
                CyberFuck.netPlay.Update(gameTime);
        }

        public static void Draw(GameTime gameTime, SpriteBatch spriteBatch)
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
            map.Draw(gameTime);
            lock (players)
            {
                foreach (var player in players.Values.ToArray())
                {
                    player.Draw(gameTime);
                }
            }
            spriteBatch.End();
        }

    }
}
