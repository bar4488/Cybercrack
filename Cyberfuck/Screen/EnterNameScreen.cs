using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Cyberfuck.Screen
{
    class EnterNameScreen: MenuScreen
    {
        public delegate void OnNameChosen(string name);
        OnNameChosen onNameChosen;
        enum State
        {
            Name,
            Continue
        }
        State state = State.Name;
        string name = "";
        bool error = false;
        public EnterNameScreen(OnNameChosen callback)
        {
            this.onNameChosen = callback;
            ReloadTexts();
        }
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.Draw(gameTime, spriteBatch);
        }

        private void ReloadTexts()
        {
            texts.Clear();
            texts.Add("Name: " + name);
            texts.Add("Continue");
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
                case State.Name:
                    name = Input.InputString(name);
                    ReloadTexts();
                    break;
                case State.Continue:
                    break;
                default:
                    break;
            }
            if (Input.KeyWentDown(Keys.Enter))
            {
                switch ((State)(choice % texts.Count))
                {
                    case State.Name:
                        choice++;
                        break;
                    case State.Continue:
                        if(name != "")
                        {
                        this.onNameChosen(name);
                        }
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
