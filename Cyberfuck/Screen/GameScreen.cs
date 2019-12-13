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
        public GameScreen(string level)
        {
            if(!NetStatus.Client)
                World.LoadWorld(level);
        }
        public GameScreen()
        {
            if(!NetStatus.Client)
                World.LoadWorld();
        }
        public void Update(GameTime gameTime)
        {
			World.Update(gameTime);

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
            spriteBatch.DrawString(CyberFuck.font, String.Format("{0} players connected", NetStatus.playersCount), Vector2.Zero, Color.Black, 0, Vector2.Zero, 0.5f, SpriteEffects.None, 0);
            spriteBatch.End();
        }

        public void Close(OnClose callBack)
        {
            CyberFuck.netPlay?.Close();
            CyberFuck.Screen = new MainScreen();
            // close the game;
            callBack?.Invoke();
        }
    }
}
