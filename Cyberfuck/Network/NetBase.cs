using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cyberfuck.Entities;
using Cyberfuck.Data;

namespace Cyberfuck.Network
{
    public abstract class NetBase : INetBase
    {
        public void SendMessage(MessageContentType messageType, int player, IMessageContent data)
        {
            NetworkMessage msg = new NetworkMessage(data);
            SendBuffer(msg.Encode(), player);
        }

        public abstract void SendBuffer(byte[] msg, int player);
        public abstract void SnapShot();

        public abstract void Close();

        public abstract void Update();
    }
}
