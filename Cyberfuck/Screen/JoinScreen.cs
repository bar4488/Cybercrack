using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Cyberfuck.Screen
{
    class JoinScreen: IScreen
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
                string text = value.ToString();
                if ((State)value == State.IP)
                    text += ": " + ip;
                if ((State)value == State.Port)
                    text += ": " + port;
                spriteBatch.DrawString(CyberFuck.font, text, new Vector2(CenterX(text), CyberFuck.graphics.GraphicsDevice.Viewport.Height / 2 -70 + count * 70), state == (State)value ? Color.Red : Color.Black, 0, Vector2.Zero, 2, SpriteEffects.None, 0f);
                count++;
            }
            if(error)
                spriteBatch.DrawString(CyberFuck.font, "could not connect to host", new Vector2(10, CyberFuck.graphics.GraphicsDevice.Viewport.Height -70),  Color.Red, 0, Vector2.Zero, 2, SpriteEffects.None, 0f);
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

            switch (state)
            {
                case State.IP:
                    ip = Input.InputString(ip);
                    break;
                case State.Port:
                    port = Input.InputString(port);
                    break;
                case State.Join:
                    break;
                default:
                    break;
            }
            if (Input.KeyWentDown(Keys.Enter))
            {
                switch (state)
                {
                    case State.IP:
                        state = State.Port;
                        break;
                    case State.Port:
                        state = State.Join;
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
