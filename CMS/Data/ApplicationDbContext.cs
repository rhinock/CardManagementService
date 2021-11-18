using Microsoft.EntityFrameworkCore;
using System;
using CMS.UI.Models;

namespace CMS.UI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }
        
        public DbSet<Card> Cards { get; set; }
    }
}
