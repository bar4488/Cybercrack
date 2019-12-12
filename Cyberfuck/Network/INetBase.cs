using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cyberfuck.Network
{
    public enum NetType
    {
        Server,
        Client,
        Single
    }
    public interface INetBase
    {
        void SendMessage(MessageType messageType, int player, object data);
        void Update();
        void Close();
    }

}
