using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cyberfuck.Data
{
    public enum MessageContentType
    {
        /// <summary>
        /// send update to the server about the client's player's data when changed
        /// </summary>
        PlayerUpdate,
        /// <summary>
        /// message containing data about entities in the world
        /// </summary>
        EntityData,
        /// <summary>
        /// broadcast from the server to indicate that a player has left
        /// </summary>
        RemovePlayer,
        /// <summary>
        /// notify when the server or the client are closing their connection
        /// </summary>
        CloseConnection,

    }
}
