using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Cyberfuck.Entities;
using Cyberfuck.Data;
using Cyberfuck.Network;

namespace Cyberfuck.GameWorld
{
    public class World
    {
        public Player Player { get => Players[MyPlayerId]; }
        public NetType NetType { get; set; }
        public WorldMap Map { get; set; }
        public Camera2D Camera { get; set; }
        public int MyPlayerId { get; set; }
        public Dictionary<int, Player> Players { get; set; } = new Dictionary<int, Player>();
        public List<IEntity> Entities { get; set; } = new List<IEntity>();
        public Humper.World CollisionWorld { get; set; }

        public World()
        {

        }

        public World(int myPlayerId, Dictionary<int, Player> players)
        {
            this.MyPlayerId = myPlayerId;
            this.Players = players;
        }

        public void LoadWorld(string level = "Level3.png")
        {
            this.Players.Clear();
            this.Entities.Clear();
            this.Map = new WorldMap(this, level);
            this.CollisionWorld = Map.collisionWorld;
            if(NetType == NetType.Single || NetType == NetType.Server)
            {
                this.MyPlayerId = -1;
                this.Players[-1] = new Player(this, MyPlayerId);
            }
            this.Camera = new Camera2D();
            this.Camera.Focus = Players[MyPlayerId];
        }
        public void LoadWorld(System.Drawing.Bitmap level)
        {
            Players.Clear();
            Entities.Clear();
            Map = new WorldMap(this, level);
            CollisionWorld = Map.collisionWorld;
            if(NetType == NetType.Single || NetType == NetType.Server)
            {
                MyPlayerId = -1;
                Players[-1] = new Player(this, MyPlayerId);
                Camera = new Camera2D();
                Camera.Focus = Players[MyPlayerId];
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
            lock (Players)
            {
                Players[playerData.ID] = new Player(this, playerData);
                if (local)
                {
                    MyPlayerId = playerData.ID;
                    Camera = new Camera2D();
                    Camera.Focus = Players[MyPlayerId];
                }
            }
        }
        public void RemovePlayer(int id)
        {
            lock (Players)
            {
                Players.Remove(id);
            }
        }
        public void Update(GameTime gameTime)
        {
            if(NetType != NetType.Single)
                CyberFuck.netPlay.SnapShot();
            lock (Players)
            {
                foreach (var player in Players.Values.ToArray())
                {
                    player.Update(gameTime);
                }
            }
            Camera.Update(gameTime);
            if(NetType != NetType.Single)
                CyberFuck.netPlay.Update(gameTime);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
			spriteBatch.Begin(
				SpriteSortMode.Deferred,
				BlendState.AlphaBlend,
				SamplerState.LinearClamp,
				DepthStencilState.None,
				RasterizerState.CullCounterClockwise,
				null,
				Camera.Transform
			);
            Map.Draw(gameTime);
            lock (Players)
            {
                foreach (var player in Players.Values.ToArray())
                {
                    player.Draw(gameTime);
                }
            }
            spriteBatch.End();
        }

    }
}
