using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cyberfuck
{
    public class PlayerData
    {
        public EntityData Entity;
        public int ID;

        public PlayerData(Player player)
        {
            Entity = new EntityData(player);
            ID = player.ID;
        }
        public static bool operator ==(PlayerData d, Player o)
        {
            return d.Entity == o;
        }

        public static bool operator !=(PlayerData d, Player o)
        {
            return !(d.Entity==o);
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
