using System.Collections.Generic;

namespace LoggerService
{
    public class AppContext
    {
        static AppContext()
        {
            Logs = new List<Log>();
        }

        public static List<Log> Logs { get; private set; }
    }
}
