using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cyberfuck.Logger
{
    public class ConsoleLogger : ILogger
    {
        public void Log(object log)
        {
            Console.WriteLine(log);
        }
        public void Log(string tag, object log)
        {
            Console.Write(tag + ": ");
            Console.WriteLine(log);
        }
    }
}
