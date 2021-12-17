using Domain.Interfaces;
using Domain.Objects;
using PgDataStore;

namespace Infrastructure
{
    public static class DataToolManager
    {
        public static IDataTool GetDataTool<T>(ResourceConnection connection) where T : class, IDataObject
        {
            return new DataContext<T>(connection.Value);
        }

        public static IDataTool DataTool<T>(this ResourceConnection connection) where T : class, IDataObject
        {
            return GetDataTool<T>(connection);
        }
    }
}
