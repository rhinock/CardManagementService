using System;
using System.Linq;
using System.Collections.Generic;

using PgDataStore;

using Data.BalancerService.Objects;

using Microsoft.EntityFrameworkCore;

namespace Data.BalancerService
{
    public class MigrationContext : DbContext, IMigrationDataContext
    {
        public MigrationContext(DbContextOptions<MigrationContext> options) : base(options)
        {
        }

        public virtual DbSet<Route> Route { get; set; }

        public void InitialData(object preData)
        {
            if(!(preData is IEnumerable<Route> routes))
            {
                throw new ArgumentException("preData");
            }

            if(!Route.Any())
            {
                Route.AddRange(routes);
                SaveChanges();
            }
        }

        public void Migrate()
        {
            Database.Migrate();
        }
    }

    public class MigrationContextFactory : MigrationDataContextFactory<MigrationContext>
    {
    }
}