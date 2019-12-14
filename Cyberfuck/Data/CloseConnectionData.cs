using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cyberfuck.Network;
using System.Runtime.Serialization;

namespace Cyberfuck.Data
{
    class CloseConnectionData : IMessageContent
    {

        public byte[] Encode()
        {
            throw new NotImplementedException();
        }
    }
}
