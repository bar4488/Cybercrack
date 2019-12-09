using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Cyberfuck
{
    interface IGameObject: IGameComponent
    {
        CyberfuckGame Game { get; }
    }

    class GameObject: GameComponent, IGameObject
    {
        public new CyberfuckGame Game { get => base.Game as CyberfuckGame; }

        public GameObject(): base(CyberfuckGame.GetInstance())
        {
        }
    }

    class DrawableGameObject : DrawableGameComponent, IGameObject
    {
        public new CyberfuckGame Game { get => base.Game as CyberfuckGame; }

        public DrawableGameObject() : base(CyberfuckGame.GetInstance())
        {
        }
    }
}
