using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace PgDataStore
{
    public class DataContext<T> : DbContext where T : class, IDataObject
    {
        private readonly string _connectionString;

        public DataContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(_connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<T>();
        }
    }
}