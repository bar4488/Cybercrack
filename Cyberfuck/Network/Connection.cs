using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace Cyberfuck.Network
{
    enum ConnectionState
    {
        Connected,
        Initialized,
        NotConnected
    }
    class Connection
    {
        ConnectionState state;
        TcpClient conn;
        NetworkStream stream;

        public bool Connected { get => state == ConnectionState.Connected; }
        public ConnectionState State { get => state; }
        public NetworkStream Stream { get => stream; set => stream = value; }

        public Connection(TcpClient conn)
        {
            this.conn = conn;
            stream = conn.GetStream();
            state = ConnectionState.Connected;
        }
        public Connection()
        {
            state = ConnectionState.NotConnected;
        }

        /// <summary>
        /// check if any new data from the client is avalible and process it
        /// </summary>
        public void Update()
        {

        }
        
        public void Reset(TcpClient conn)
        {
            state = ConnectionState.Initialized;
            this.conn = conn;
            this.stream = conn.GetStream();
        }
    }
}
