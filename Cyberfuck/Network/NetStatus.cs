using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cyberfuck.Network
{
    public static class NetStatus
    {
        public static bool Single { get => Type == NetType.Single; }
        public static bool Client { get => Type == NetType.Client; }
        public static bool Server { get => Type == NetType.Server; }
        public static int playersCount;
        public static NetType Type = NetType.Single;

    }
}
