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
        public event OnCloseEvent OnClose;
        protected World world;

        public NetBase(World world)
        {
            this.world = world;
        }
        public void SendMessage(MessageContentType type, int player, IMessageContent data)
        {
            NetworkMessage msg;
            if(data != null)
            {
                msg = new NetworkMessage(data, type);
                SendBuffer(msg.Encode(), player);
            }
            else
            {
                SendBuffer(NetworkMessage.EmptyMessage(type), player);
            }
        }

        public abstract void SendBuffer(byte[] msg, int player);
        public abstract void SnapShot();

        public virtual void Close(CloseReason reason)
        {
            OnClose?.Invoke(reason);
            CyberFuck.netPlay?.SendMessage(MessageContentType.CloseConnection, NetStatus.Server ? -1 : world.MyPlayerId, null);
        }
        public abstract void Update(GameTime gameTime);
    }
}
