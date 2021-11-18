using Microsoft.EntityFrameworkCore;
using System;
using WebApplication8.Models;

namespace WebApplication8.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }
        
        public DbSet<Card> Cards { get; set; }
    }
}
