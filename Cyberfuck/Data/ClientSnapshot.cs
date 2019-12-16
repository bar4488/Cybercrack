using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cyberfuck.Data
{
    class ClientSnapshot
    {
        public PlayerData playerData;
        public ClientSnapshot(PlayerData playerData)
        {
            this.playerData = playerData;
        }
        public static ClientSnapshot SnapShot(World world)
        {
            PlayerData myPlayer = new PlayerData(world.Player);
            return new ClientSnapshot(myPlayer);
        }
    }
}
