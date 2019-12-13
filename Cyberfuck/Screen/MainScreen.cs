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
            Single,
            Quit
        }
        int state = Enum.GetValues(typeof(State)).Length * 100000;
        public MainScreen()
        {

        }
        public int ToState(int num)
        {
            return num % Enum.GetValues(typeof(State)).Length;
        }

        public void Close(OnClose callback)
        {
            callback();
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            int count = 0;
            foreach (var value in Enum.GetValues(typeof(State)))
            {
                spriteBatch.DrawString(CyberFuck.font, value.ToString(), new Vector2(CenterX(value.ToString()), CyberFuck.graphics.GraphicsDevice.Viewport.Height / 2 -70 + count * 70), ToState(state) == (int)value ? Color.Red : Color.Black, 0, Vector2.Zero, 2, SpriteEffects.None, 0f);
                count++;
            }
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
                state--;
            if (Input.KeyWentDown(Keys.Down))
                state++;

			if(Input.KeyWentDown(Keys.Escape))
			{
                Close(() => {
                    CyberFuck.instance.Exit();
                });
			}
            if (Input.KeyWentDown(Keys.Enter))
            {
                switch ((State)(ToState(state)))
                {
                    case State.Host:
                        CyberFuck.Screen = new ChooseWorldScreen((string world) =>
                        {
                            CyberFuck.Host(world);
                        });
                        break;
                    case State.Join:
                        CyberFuck.Screen = new JoinScreen();
                        break;
                    case State.Single:
                        CyberFuck.Screen = new ChooseWorldScreen((w) =>
                        {
                            CyberFuck.Start(w);
                        });
                        break;
                    case State.Quit:
                        CyberFuck.instance.Exit();
                        break;
                    default:
                        break;
                }
            }
        }

        public void OnChooseWorld(string world)
        {
            CyberFuck.Host(world);
        }
    }
}
