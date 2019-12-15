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
    public class ChooseWorldScreen: MenuScreen
    {
        public delegate void OnWorldChosen(string world);
        OnWorldChosen onWorldChosen;

        public ChooseWorldScreen(OnWorldChosen callback): base()
        {
            this.onWorldChosen = callback;
            foreach (string file in Directory.EnumerateFiles("Content/Levels"))
            {
                string fileName = file.Split('/', '\\').Last();
                texts.Add(fileName);
                CyberFuck.Logger.Log(fileName);
            }
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
            if (Input.KeyWentDown(Keys.Enter))
            {
                this.onWorldChosen(texts[choice % texts.Count]);
            }
        }
    }
}
