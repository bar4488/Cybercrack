using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Cyberfuck.Screen
{
    class JoinScreen: MenuScreen
    {
        enum State
        {
            IP,
            Port,
            Join
        }
        State state = State.IP;
        string ip = "127.0.0.1";
        string port = "1234";
        bool error = false;
        public JoinScreen()
        {
            ReloadTexts();
        }
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.Draw(gameTime, spriteBatch);

            spriteBatch.Begin();
            if(error)
                spriteBatch.DrawString(CyberFuck.font, "could not connect to host", new Vector2(10, CyberFuck.graphics.GraphicsDevice.Viewport.Height -70),  Color.Red, 0, Vector2.Zero, 2, SpriteEffects.None, 0f);
            spriteBatch.End();
        }

        private void ReloadTexts()
        {
            texts.Clear();
            texts.Add("IP: " + ip);
            texts.Add("Port: " + port);
            texts.Add("Join");
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
			if(Input.KeyWentDown(Keys.Escape))
			{
                Close(() => {
                    CyberFuck.Screen = new MainScreen();
                });
			}
            switch ((State)(choice % texts.Count))
            {
                case State.IP:
                    ip = Input.InputString(ip);
                    ReloadTexts();
                    break;
                case State.Port:
                    port = Input.InputString(port);
                    ReloadTexts();
                    break;
                case State.Join:
                    break;
                default:
                    break;
            }
            if (Input.KeyWentDown(Keys.Enter))
            {
                switch ((State)(choice % texts.Count))
                {
                    case State.IP:
                        choice++;
                        break;
                    case State.Port:
                        choice++;
                        break;
                    case State.Join:
                        int portNum;
                        if (int.TryParse(port, out portNum))
                            try
                            {
                                CyberFuck.Join(ip, portNum);
                            }
                            catch (System.Net.Sockets.SocketException)
                            {
                                error = true;
                            }
                        else
                            port = "";
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
