using System;
using System.Linq;
using System.Collections.Generic;

using PgDataStore;

using Data.OperationDataService.Objects;

using Microsoft.EntityFrameworkCore;

namespace Data.OperationDataService
{
    public class MigrationContext : DbContext, IMigrationDataContext
    {
        public MigrationContext(DbContextOptions<MigrationContext> options) : base(options)
        {
        }

        public virtual DbSet<Operation> Operation { get; set; }

        public void InitialData(object preData)
        {
            if(!(preData is IEnumerable<Operation> operations))
            {
                throw new ArgumentException("preData");
            }

            if(!Operation.Any())
            {
                Operation.AddRange(operations);
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