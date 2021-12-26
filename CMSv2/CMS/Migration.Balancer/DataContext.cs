using Data.Balancer.Objects;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System.Linq;

namespace Data.Balancer
{
    public class DataContext : DbContext, IDataTool
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public virtual DbSet<Route> Route { get; set; }

        public bool TryInitData()
        {
            Database.Migrate();
            return true;
        }
    }

    public class DataContextFactory : IDesignTimeDbContextFactory<DataContext>
    {
        public DataContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<DataContext>();
            var connection = args.FirstOrDefault();
            optionsBuilder.UseNpgsql(connection);

            return new DataContext(optionsBuilder.Options);
        }
    }
}