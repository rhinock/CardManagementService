using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using CMS.UI.Models;

namespace CMS.UI.Data
{
    public class DataGenerator
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new ApplicationDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>()))
            {
                // Look for any cards
                if (context.Cards.Any())
                {
                    return; // Data was already seeded
                }

                var card1 = new Card
                {
                    Cvc = "101",
                    Expire = DateTime.UtcNow.AddDays(1),
                    IsDefault = false,
                    Name = "test1",
                    Pan = "0000 0000 0000 0101",
                    UserId = Guid.NewGuid()
                };

                var card2 = new Card
                {
                    Cvc = "102",
                    Expire = DateTime.UtcNow.AddMonths(1),
                    IsDefault = false,
                    Name = "test2",
                    Pan = "0000 0000 0000 0102",
                    UserId = Guid.NewGuid()
                };

                context.Cards.AddRange(new Card[] { card1, card2 });
                context.SaveChanges();
            }
        }
    }
}
