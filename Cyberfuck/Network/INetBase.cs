using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cyberfuck.Data;
using Microsoft.Xna.Framework;

namespace Cyberfuck.Network
{
    public enum NetType
    {
        Server,
        Client,
        Single
    }
    public enum CloseReason: byte
    {
        ConnectionInterrupt,
        UserLeft,
        ServerClosed,
    }
    public delegate void OnCloseEvent(CloseReason reason);
    public interface INetBase
    {
        event OnCloseEvent OnClose;
        void SendMessage(MessageContentType messageType, int player, IMessageContent data);
        void Update(GameTime gameTime);
        void Close(CloseReason reason);
        void SnapShot();
    }

}
