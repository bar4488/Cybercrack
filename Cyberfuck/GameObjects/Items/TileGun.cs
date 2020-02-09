using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cyberfuck.GameWorld;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Cyberfuck.GameObjects.Items
{
    class TileGun: Gun
    {
        int gravity = 1;
        public override int ItemID => 3;
        public override Texture2D ShotTexture => CyberFuck.GetTexture("tileDirt");
        public TileGun(World world, IEntity entity): base(world, entity)
        {
            
        }

        public override bool MoveShot(VelocityObject shot)
        {
            shot.Velocity.Y += gravity;
            bool shouldRemove = false;
            bool placeDirt = false;
            Vector2 oldCenter = shot.Center;
            shot.Box.Move(shot.Position.X + shot.Velocity.X, shot.Position.Y + shot.Velocity.Y, (col) => {
                if (col.Other.HasTags(Collider.Tile))
                {
                    shouldRemove = true;
                    placeDirt = true;
                    return new Humper.Responses.TouchResponse(col);
                }
                if (col.Other.HasTags(Collider.Player))
                {
                    if(Holder.ID != (int)col.Other.Data)
                    {
                        world.Players[(int)col.Other.Data].Damage(damageAmount, DamageReason.Enemy, "a gun shot you");
                        shouldRemove = true;
                    }
                }
                return new Humper.Responses.IgnoreResponse(col);
            });
            if(shouldRemove && placeDirt)
            {
                world.AddTile((int)(shot.Center.X / 16), (int)(shot.Center.Y / 16), Tile.Dirt);
            }
            return shouldRemove;
        }
    }
}
