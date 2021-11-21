using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using CMS.UI.Data;
using CMS.UI.Models;

namespace CMS.UI.Middleware
{
    public class ConventionalMiddleware
    {
        private readonly RequestDelegate _next;

        public ConventionalMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        [HttpPost]
        public async Task InvokeAsync(HttpContext context, ApplicationDbContext db)
        {
            var keyValue = context.Request.Query["ConventionalMiddleware"];

            if (!string.IsNullOrWhiteSpace(keyValue))
            {
                db.Add(new Card()
                {
                    Cvc = "201",
                    Expire = DateTime.UtcNow.AddYears(1),
                    IsDefault = true,
                    Name = keyValue,
                    Pan = "0000 0000 0000 0201",
                    UserId = Guid.NewGuid()
                });

                await db.SaveChangesAsync();
            }

            await _next(context);
        }
    }
}
