using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.IO;

namespace Cyberfuck.Screen
{
    public class ChooseWorldScreen: IScreen
    {
        public delegate void OnWorldChosen(string world);
        OnWorldChosen onWorldChosen;
        int worldChoice = 0;
        List<string> worlds;

        string ip = "127.0.0.1";
        string port = "1234";
        public ChooseWorldScreen(OnWorldChosen callback)
        {
            this.onWorldChosen = callback;
            this.worlds = new List<string>();
            foreach (string file in Directory.EnumerateFiles("Content/Levels"))
            {
                string fileName = file.Split('/', '\\').Last();
                worlds.Add(fileName);
                CyberFuck.Logger.Log(fileName);
            }

        }
        public void Close(OnClose callback)
        {
            callback();
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            int count = 0;
            foreach (var value in worlds)
            {
                string text = value;
                spriteBatch.DrawString(CyberFuck.font, text, new Vector2(CenterX(text), CyberFuck.graphics.GraphicsDevice.Viewport.Height / 2 -70 + count * 70), count == worldChoice % worlds.Count ? Color.Red : Color.Black, 0, Vector2.Zero, 2, SpriteEffects.None, 0f);
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
                worldChoice--;
            if (Input.KeyWentDown(Keys.Down))
                worldChoice++;
            if (Input.KeyWentDown(Keys.Enter))
            {
                this.onWorldChosen(worlds[worldChoice % worlds.Count]);
            }
        }
    }
}
