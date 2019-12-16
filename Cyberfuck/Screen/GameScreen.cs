using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Cyberfuck.Network;

namespace Cyberfuck.Screen
{
    public class GameScreen: IScreen
    {
        World world;

        public World World { get => world; }

        public GameScreen(string level)
        {
            world = new World();
            world.LoadWorld(level);
        }
        public GameScreen(World world)
        {
            this.world = world;
        }
        public void Update(GameTime gameTime)
        {
			world.Update(gameTime);

			if(Input.KeyWentDown(Keys.Escape))
			{
                Close(() => { });
			}
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Rectangle mainFrame = new Rectangle(0, 0, CyberFuck.graphics.GraphicsDevice.Viewport.Width, CyberFuck.graphics.GraphicsDevice.Viewport.Height);
            spriteBatch.Begin();
            spriteBatch.Draw(CyberFuck.textures["background"], mainFrame, Color.White);
            spriteBatch.End();
            World.Draw(gameTime, spriteBatch);
            spriteBatch.Begin();
            spriteBatch.DrawString(CyberFuck.font, String.Format("{0} players connected", world.Players.Count), Vector2.Zero, Color.Black, 0, Vector2.Zero, 0.5f, SpriteEffects.None, 0);
            spriteBatch.End();
        }

        public void Close(OnClose callBack)
        {
            CyberFuck.netPlay?.Close(CloseReason.UserLeft);
            CyberFuck.Screen = new MainScreen();
            // close the game;
            callBack?.Invoke();
        }
    }
}
