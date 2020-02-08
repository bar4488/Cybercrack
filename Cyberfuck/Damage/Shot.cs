using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Cyberfuck.GameWorld;


namespace Cyberfuck.Damage
{
    class Shot
    {
        DamageField damage;

        public DamageField Damage { get => damage; set => damage = value; }
        public Texture2D Texture { get => CyberFuck.textures["shot"]; }

        public Shot(World world, Vector2 position, Vector2 velocity)
        {
            damage = new DamageField(world, position, velocity, new Vector2(Texture.Width, Texture.Height));
        }

        public void Update(GameTime gameTime)
        {
            damage.Update(gameTime);
        }

    }
}
