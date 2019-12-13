using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cyberfuck.Entities;
using Cyberfuck.Data;
using Microsoft.Xna.Framework;

namespace Cyberfuck.Network
{
    public abstract class NetBase : INetBase
    {
        public void SendMessage(MessageContentType messageType, int player, IMessageContent data)
        {
            NetworkMessage msg;
            if(data != null)
            {
                msg = new NetworkMessage(data);
                SendBuffer(msg.Encode(), player);
            }
            else
            {
                SendBuffer(NetworkMessage.EmptyMessage(messageType), player);
            }
        }

        public abstract void SendBuffer(byte[] msg, int player);
        public abstract void SnapShot();

        public virtual void Close()
        {
            SendMessage(MessageContentType.RemovePlayer, NetStatus.Server ? -1 : World.myPlayerId, null);
        }

        public abstract void Update(GameTime gameTime);
    }
}
