using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Cyberfuck.Network;

namespace Cyberfuck.Screen
{
    public class GameScreen: IScreen
    {
        public GameScreen()
        {
            if(!NetStatus.Client)
                World.LoadWorld();
        }
        public void Update(GameTime gameTime)
        {
			World.Update(gameTime);

        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
			World.Draw(spriteBatch);
        }

        public void Close(OnClose callBack)
        {
            // close the game;
            callBack();
        }
    }
}
