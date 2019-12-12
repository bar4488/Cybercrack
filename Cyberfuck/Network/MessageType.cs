using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cyberfuck.Network
{
    public enum MessageType
    {
        /// <summary>
        /// send update to the server about the client's player's data when changed
        /// </summary>
        PlayerUpdate,
        /// <summary>
        /// message containing data about entities in the world
        /// </summary>
        EntityData,
    }
}
