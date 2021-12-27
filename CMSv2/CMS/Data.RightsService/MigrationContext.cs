using System;
using System.Linq;
using System.Collections.Generic;

using PgDataStore;

using Data.RightsService.Objects;

using Microsoft.EntityFrameworkCore;

namespace Data.RightsService
{
    public class MigrationContext : DbContext, IMigrationDataContext
    {
        public MigrationContext(DbContextOptions<MigrationContext> options) : base(options)
        {
        }

        public virtual DbSet<User> User { get; set; }

        public void InitialData(object preData)
        {
            if(!(preData is IEnumerable<User> users))
            {
                throw new ArgumentException("preData");
            }

            if(!User.Any())
            {
                User.AddRange(users);
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