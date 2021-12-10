using PgDataStore;

namespace CardDataService
{
    public class AppDataContext : DataContext
    {
        public AppDataContext(string path) : base(path)
        {
        }
    }
}
