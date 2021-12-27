using System;
using System.Linq;
using System.Collections.Generic;

using PgDataStore;

using Data.CardDataService.Objects;

using Microsoft.EntityFrameworkCore;

namespace Data.CardDataService
{
    public class MigrationContext : DbContext, IMigrationDataContext
    {
        public MigrationContext(DbContextOptions<MigrationContext> options) : base(options)
        {
        }

        public virtual DbSet<Card> Card { get; set; }

        public void InitialData(object preData)
        {
            if(!(preData is IEnumerable<Card> cards))
            {
                throw new ArgumentException("preData");
            }

            if(!Card.Any())
            {
                Card.AddRange(cards);
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