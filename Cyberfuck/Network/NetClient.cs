using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace Cyberfuck.Network
{
    public class NetClient : NetBase
    {
        Connection server;
        public NetType Type => throw new NotImplementedException();

        public NetClient(string ip, int port)
        {
            TcpClient serverTcp = new TcpClient(ip, port);
            server = new Connection(serverTcp);
        }
        public override void SendBuffer(byte[] msg, int player)
        {
            server.Stream.Write(msg, 0, msg.Length);
        }
        public override void Close()
        {
            throw new NotImplementedException();
        }
        public override void Update()
        {
            throw new NotImplementedException();
        }
    }
}
