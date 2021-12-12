using Domain.Enums;
using Domain.Objects;
using Domain.Interfaces;

using FileDataStore;

namespace Infrastructure
{
    public static class LoggerManager
    {
        public static ILogger GetLogger(ResourceConnection connection)
        {
            switch (connection.Type)
            {
                case ConnectionType.Data:
                    return new FileLogger(connection);
                case ConnectionType.Service:
                    //return new DataServiceClient(connection);
                    return null;
                default:
                    return null;

            }
        }

        public static ILogger Logger(this ResourceConnection connection)
        {
            return GetLogger(connection);
        }
    }
}