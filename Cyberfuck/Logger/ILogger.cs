using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cyberfuck.Logger
{
    public interface ILogger
    {
        void Log(object log);
        void Log(string tag, object log);
    }
}
