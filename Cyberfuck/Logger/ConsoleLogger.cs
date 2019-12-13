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
            Log(tag + ": " + log);
        }

        public void Log(int priority, object log)
        {
            string textPriority = PriorityText(priority);
            Log(textPriority + ": " + log);
        }

        public void LogErr(int priority, string err)
        {
            Log(PriorityText(priority) + " error!: " + err);
        }

        public string PriorityText(int priority)
        {
            string textPriority;
            switch (priority)
            {
                case 0:
                    textPriority = "not important";
                    break;
                case 1:
                    textPriority = "lowest priority";
                    break;
                case 2:
                    textPriority = "low priority";
                    break;
                case 3:
                    textPriority = "medium priority";
                    break;
                case 4:
                    textPriority = "high priority";
                    break;
                case 5:
                    textPriority = "very high priority";
                    break;
                case 6:
                    textPriority = "urgent!";
                    break;
                default:
                    textPriority = "";
                    break;
            }
            return textPriority;
        }
    }
}
