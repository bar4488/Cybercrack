using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace Cyberfuck.Screen
{
    class MainScreen : IScreen
    {
        enum State
        {
            Host,
            Join,
            Single
        }
        State state = State.Host;
        public MainScreen()
        {

        }
        public void Close(IScreen.OnClose callback)
        {
            callback();
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.DrawString(CyberFuck.font, "Host", new Vector2(CenterX("Host"), CyberFuck.graphics.GraphicsDevice.Viewport.Height / 2 - 70), state == State.Host ? Color.Red : Color.Black, 0, Vector2.Zero, 2, SpriteEffects.None, 0f);
            spriteBatch.DrawString(CyberFuck.font, "Join", new Vector2(CenterX("Join"), CyberFuck.graphics.GraphicsDevice.Viewport.Height / 2), state == State.Join ? Color.Red : Color.Black, 0, Vector2.Zero, 2, SpriteEffects.None, 0f);
            spriteBatch.DrawString(CyberFuck.font, "Single", new Vector2(CenterX("Single"), CyberFuck.graphics.GraphicsDevice.Viewport.Height / 2 + 70), state == State.Single ? Color.Red : Color.Black, 0, Vector2.Zero, 2, SpriteEffects.None, 0f);
            spriteBatch.End();
        }

        public int CenterX(string text)
        {
            int posX = CyberFuck.graphics.GraphicsDevice.Viewport.Width / 2;
            return posX - (int)CyberFuck.font.MeasureString(text).X / 2;
        }

        public void Update(GameTime gameTime)
        {
            if (Input.KeyWentDown(Keys.Up))
                state = state > 0 ? state - 1 : state;
            if (Input.KeyWentDown(Keys.Down))
                state = (int)state < 2 ? state + 1 : state;

            if (Input.KeyWentDown(Keys.Enter))
            {
                switch (state)
                {
                    case State.Host:
                        CyberFuck.Host("Level1");
                        break;
                    case State.Join:
                        CyberFuck.Join("127.0.0.1", 1234);
                        break;
                    case State.Single:
                        CyberFuck.Start("Level1");
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
