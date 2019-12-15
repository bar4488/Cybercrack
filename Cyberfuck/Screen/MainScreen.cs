using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace Cyberfuck.Screen
{
    class MainScreen : MenuScreen 
    {
        enum State
        {
            Host,
            Join,
            Single,
            Quit
        }
        public MainScreen()
        {
            texts.AddRange(Enum.GetNames(typeof(State)));
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

			if(Input.KeyWentDown(Keys.Escape))
			{
                Close(() => {
                    CyberFuck.instance.Exit();
                });
			}
            if (Input.KeyWentDown(Keys.Enter))
            {
                switch ((State)(choice % texts.Count))
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
