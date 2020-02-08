using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Cyberfuck.GameObjects
{
    public interface IItem: IGameObject
    {
        int ItemID { get; }
        int OwnerID { get; set; }
        bool IsOwned { get; set; }
        Texture2D Texture { get; }

        void Use(GameTime gameTime, Vector2 MousePosition);
        void SecondUse(GameTime gameTime, Vector2 MousePosition);
    }
}
