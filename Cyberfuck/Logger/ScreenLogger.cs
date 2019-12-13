using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cyberfuck.Logger
{
    class ScreenLogger : ILogger
    {
        public void Log(object log)
        {
            throw new NotImplementedException();
        }

        public void Log(string tag, object log)
        {
            throw new NotImplementedException();
        }

        public void Log(int priority, object log)
        {
            throw new NotImplementedException();
        }

        public void LogErr(int priority, string err)
        {
            throw new NotImplementedException();
        }
    }
}
