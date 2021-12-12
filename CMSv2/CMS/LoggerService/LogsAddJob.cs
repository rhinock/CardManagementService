using Infrastructure;

using Domain.Objects;
using Domain.Interfaces;

using System.Threading;
using LoggerService.Enums;

namespace LoggerService
{
    public class LogsAddJob
    {
        private readonly ILogger _logger;
        private readonly int _timeout;

        public LogsAddJob(ResourceConnection connection, int timeout)
        {
            _timeout = timeout;
            _logger = LoggerManager.GetLogger(connection);
        }

        public void Run()
        {
            new Thread(Do).Start();
        }

        private void Do()
        {
            while(true)
            {
                try
                {
                    lock (AppContext.Logs)
                    {
                        if (AppContext.Logs?.Count > 0)
                        {
                            AppContext.Logs.ForEach(x =>
                            {
                                switch(x.Type)
                                {
                                    case LogType.Info:
                                        _logger.Info(x.Content);
                                        break;
                                    case LogType.Error:
                                        _logger.Error(x.Content);
                                        break;
                                }
                            });
                            AppContext.Logs.Clear();
                        }
                    }
                }
                catch { }
                
                Thread.Sleep(_timeout);
            }
        }
    }
}
