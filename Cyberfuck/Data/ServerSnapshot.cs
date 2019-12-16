using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cyberfuck.Entities;

namespace Cyberfuck.Data
{
    struct ServerSnapshot
    {
        public Dictionary<int, PlayerData> playersData;
        public List<EntityData> entitiesData;
        public ServerSnapshot(Dictionary<int, PlayerData> pData, List<EntityData> eData)
        {
            this.playersData = pData;
            this.entitiesData = eData;
        }
        public static ServerSnapshot Snapshot(World world)
        {
            Dictionary<int, PlayerData> playersData = new Dictionary<int, PlayerData>();
            List<EntityData> entitiesData = new List<EntityData>();
            foreach (var playerKV in world.Players)
            {
                playersData.Add(playerKV.Key, new PlayerData(playerKV.Value));
            }
            foreach (var entity in world.Entities)
            {
                entitiesData.Add(new EntityData(entity));
            }
            return new ServerSnapshot(playersData, entitiesData);
        }
    }
}
