using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cyberfuck.GameWorld;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Cyberfuck.GameObjects.Items
{
    class DetonatorGun: Gun
    {
        int gravity = 1;
        public override int ItemID => 4;
        public override Texture2D ShotTexture => CyberFuck.GetTexture("bomb");
        public DetonatorGun(World world, IEntity entity): base(world, entity)
        {
            
        }

        public override bool MoveShot(VelocityObject shot)
        {
            shot.Velocity.Y += gravity;
            bool shouldRemove = false;
            bool placeDirt = false;
            Vector2 tilePlace = Vector2.Zero;
            shot.Box.Move(shot.Position.X + shot.Velocity.X, shot.Position.Y + shot.Velocity.Y, (col) => {
                if (col.Other.HasTags(Collider.Tile))
                {
                    shouldRemove = true;
                    placeDirt = true;
                    tilePlace = new Vector2(col.Other.X, col.Other.Y);
                    return new Humper.Responses.TouchResponse(col);
                }
                if (col.Other.HasTags(Collider.Player))
                {
                    if(Holder.ID != (int)col.Other.Data)
                    {
                        world.Players[(int)col.Other.Data].Damage(damageAmount, DamageReason.Enemy, "a gun shot you", Holder.ID);
                        shouldRemove = true;
                    }
                }
                return new Humper.Responses.IgnoreResponse(col);
            });
            if(shouldRemove && placeDirt)
            {
                world.AddTile((int)(tilePlace.X / 16), (int)(tilePlace.Y / 16), Tile.None);
            }
            return shouldRemove;
        }
    }
}
