using PgDataStore;
using DataServices;

using Domain.Enums;
using Domain.Objects;
using Domain.Interfaces;

namespace Infrastructure
{
    public static class RepositoryManager
    {
        public static IRepository GetRepository(ResourceConnection connection)
        {
            switch(connection.Type)
                {
                case ConnectionType.Data:
                    return new Repository(connection);
                case ConnectionType.Service:
                    return new DataServiceClient(connection);
                default:
                    return null;

            }
        }

        public static IRepository Repository(this ResourceConnection connection)
        {
            return GetRepository(connection);
        }
    }
}
