using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Cyberfuck.GameObjects;
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
        public List<IGameObject> GameObjects { get; set; } = new List<IGameObject>();
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
            this.Map = new WorldMap(this, level);
            LoadWorld();
        }
        public void LoadWorld()
        {
            this.Players.Clear();
            this.Entities.Clear();
            this.CollisionWorld = Map.collisionWorld;
            if(NetType == NetType.Single || NetType == NetType.Server)
            {
                this.MyPlayerId = -1;
                this.Players[-1] = new Player(this, MyPlayerId);
                GameObjects.Add(Players[-1]);
                this.Camera = new Camera2D();
                this.Camera.Focus = Players[MyPlayerId];
            }
        }
        public void LoadWorld(Tile[,] map)
        {
            Map = new WorldMap(this, map);
            LoadWorld();
        }
        public void LoadWorld(System.Drawing.Bitmap level)
        {
            Map = new WorldMap(this, level);
            Players.Clear();
            Entities.Clear();
            CollisionWorld = Map.collisionWorld;
            if(NetType == NetType.Single || NetType == NetType.Server)
            {
                MyPlayerId = -1;
                Players[-1] = new Player(this, MyPlayerId);
                GameObjects.Add(Players[-1]);
                Camera = new Camera2D();
                Camera.Focus = Players[MyPlayerId];
            }
        }
        public bool AddTile(int x, int y, Tile tile)
        {
            return Map.AddTile(x, y, tile);
        }
        // TODO: ADD IMPLEMENTATION!
        public void LoadEntities(List<EntityData> entities)
        {
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
            if (local)
                MyPlayerId = playerData.ID;
            LoadPlayer(new Player(this, playerData));
        }
        public void LoadPlayer(Player player)
        {
            lock (GameObjects)
            {
                if (Players.ContainsKey(player.ID))
                {
                    GameObjects.Remove(Players[player.ID]);
                }
                GameObjects.Add(player);
                lock (Players)
                {
                    Players[player.ID] = player;
                    if (player.ID == MyPlayerId)
                    {
                        Camera = new Camera2D();
                        Camera.Focus = Players[MyPlayerId];
                    }
                }
            }
        }
        public void RemovePlayer(int id)
        {
            Player player = Players[id];
            lock (GameObjects)
            {
                Player.Remove();
                GameObjects.Remove(player);
                lock (Players)
                {
                    Players.Remove(id);
                }
            }
        }
        public void Update(GameTime gameTime)
        {
            if(NetType != NetType.Single && !(NetType == NetType.Client && Player.IsDead))
                CyberFuck.netPlay.SnapShot();
            lock (GameObjects)
            {
                foreach (var gameObj in GameObjects.ToArray())
                {
                    gameObj.Update(gameTime);
                }
            }
            if(!Player.IsDead)
                Camera.Update(gameTime);
            if(NetType != NetType.Single && !(NetType == NetType.Client && Player.IsDead))
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
            lock (GameObjects)
            {
                foreach (var gameObject in GameObjects)
                {
                    gameObject.Draw(spriteBatch, gameTime);
                }
            }
            spriteBatch.End();
            spriteBatch.Begin();
            Color a = Color.White;
            a.A = 100;
            spriteBatch.Draw(CyberFuck.GetTexture("rect"), new Rectangle(0, 0, 210 + 48 * 10, 36), a);
            spriteBatch.Draw(CyberFuck.GetTexture("rect"), new Rectangle(2, 18, Players[MyPlayerId].Health * 2, 16), Color.Green);
            spriteBatch.Draw(CyberFuck.GetTexture("rect"), new Rectangle(2 + Players[MyPlayerId].Health * 2, 18, 200 - Players[MyPlayerId].Health * 2, 16), Color.Red);
            for (int i = 0; i < 10; i++)
            {
                spriteBatch.Draw(CyberFuck.GetTexture("rect"), new Rectangle(210 + 8 + i * 48, 2, 32, 32), i == Player.SelectedItem ? Color.Yellow : Color.Blue);
                if(Player.Inventory[i] != null)
                {
                    spriteBatch.Draw(Player.Inventory[i].Texture, new Rectangle(210 + 8 + i * 48, 2, 28, 28), i == Player.SelectedItem ? Color.Yellow : Color.Blue);
                }
            }
            if (Player.IsDead)
            {
                CyberFuck.spriteBatch.DrawString(CyberFuck.font, "In my heart you shall forever remain", new Vector2(spriteBatch.GraphicsDevice.Viewport.Width / 2, 100), Color.Red);
            }
            spriteBatch.End();
        }

    }
}
