using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace Cyberfuck.Damage
{
    class Shot
    {
        DamageField damage;

        public DamageField Damage { get => damage; set => damage = value; }
        public Texture2D Texture { get => CyberFuck.textures["shot"]; }

        public Shot(Vector2 position, Vector2 velocity)
        {
            damage = new DamageField(position, velocity);
        }

        public void Update(GameTime gameTime)
        {
            damage.Update(gameTime);
        }

    }
}
