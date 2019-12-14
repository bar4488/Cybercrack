using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cyberfuck.Network
{
    public interface IMessageContent
    {
        byte[] Encode();
    }
}
