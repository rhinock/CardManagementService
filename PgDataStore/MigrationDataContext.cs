using System;
using System.Linq;
using System.Collections.Generic;

using Domain.Interfaces;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace PgDataStore
{
    public interface IMigrationDataContext
    {
        void Migrate();
        void InitialData(object preData);
    }

    public abstract class MigrationDataContextFactory<TContext> : IDesignTimeDbContextFactory<TContext> where TContext : DbContext
    {
        public TContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<TContext>();
            var connection = args.FirstOrDefault();
            optionsBuilder.UseNpgsql(connection);

            return (TContext)Activator.CreateInstance(typeof(TContext), optionsBuilder.Options);
        }
    }
}