using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cyberfuck.Data
{
    public interface IMessageContent
    {
        MessageContentType ContentType { get; }
        byte[] Encode();
    }
}
