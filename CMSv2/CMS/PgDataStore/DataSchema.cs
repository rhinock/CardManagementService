using System;

using Domain.Objects;
using Domain.Interfaces;

using Microsoft.EntityFrameworkCore;

namespace PgDataStore
{
    public class DataSchema<TContext> : IDataSchema where TContext : DbContext, IMigrationDataContext
    {
        private readonly TContext _dataContext;

        public DataSchema(ResourceConnection connection)
        {
            _dataContext = (TContext)Activator.CreateInstance(typeof(TContext), GetOptions(connection.Value));
        }

        public void Actualize(object preData)
        {
            _dataContext.Migrate();
            _dataContext.InitialData(preData);
        }

        public void Dispose()
        {
            _dataContext.Dispose();
        }

        private DbContextOptions<TContext> GetOptions(string connectionString)
        {
            var optionsBuilder = new DbContextOptionsBuilder<TContext>();
            optionsBuilder.UseNpgsql(connectionString);
            return optionsBuilder.Options;
        }
    }
}
