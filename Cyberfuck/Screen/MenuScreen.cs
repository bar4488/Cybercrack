using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Cyberfuck.Screen
{
    public abstract class MenuScreen: IScreen
    {
        protected int choice = 0;
        protected List<string> texts;
        protected int offset = 400;
        int start = 0;

        public MenuScreen()
        {
            texts = new List<string>();
        }
        public void Close(OnClose callback)
        {
            callback();
        }

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            int count = 0;
            //while(offset + 70 * (choice % texts.Count - 1 - start) > CyberFuck.graphics.GraphicsDevice.Viewport.Height - 80)
            while(choice % texts.Count - start > 3 || offset + 70 * (choice % texts.Count - 1 - start) > CyberFuck.graphics.GraphicsDevice.Viewport.Height - 80)
            {
                start++;
            }
            if (choice % texts.Count < start)
                start = choice % texts.Count;
            for (int i = start; i < texts.Count; i++)
            {
                var value = texts[i];
                string text = value;
                Vector2 position = new Vector2(CenterX(text), offset + count * 70);
                spriteBatch.DrawString(CyberFuck.font, text, position, i == choice % texts.Count ? Color.Red : Color.Black, 0, Vector2.Zero, 2, SpriteEffects.None, 0f);
                count++;
            }
            spriteBatch.End();
        }

        public int CenterX(string text)
        {
            int posX = CyberFuck.graphics.GraphicsDevice.Viewport.Width / 2;
            return posX - (int)CyberFuck.font.MeasureString(text).X / 2;
        }

        public virtual void Update(GameTime gameTime)
        {
			if(Input.KeyWentDown(Keys.Escape))
			{
                Close(() => {
                    CyberFuck.Screen = new MainScreen();
                });
			}
            if (Input.KeyWentDown(Keys.Up))
                choice--;
            if (Input.KeyWentDown(Keys.Down))
                choice++;
            if(choice < 0)
            {
                choice = texts.Count - 1;
            }
        }
    }
}
