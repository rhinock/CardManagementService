using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using CMS.UI.Data;
using CMS.UI.Models;

namespace CMS.UI.Middleware
{
    public class FactoryActivatedMiddleware : IMiddleware
    {
        private readonly ApplicationDbContext _db;

        public FactoryActivatedMiddleware(ApplicationDbContext db)
        {
            _db = db;
        }

        //public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        //{
        //    var keyValue = context.Request.Query["FactoryActivatedMiddleware"];

        //    if (!string.IsNullOrWhiteSpace(keyValue))
        //    {
        //        _db.Add(new Card()
        //        {
        //            Cvc = "301",
        //            Expire = DateTime.UtcNow.AddYears(1),
        //            IsDefault = true,
        //            Name = keyValue,
        //            Pan = "0000 0000 0000 0301",
        //            UserId = Guid.NewGuid()
        //        });

        //        await _db.SaveChangesAsync();
        //    }

        //    await next(context);
        //}

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var keyValue = context.Request.Query["FactoryActivatedMiddleware"];

            if (!string.IsNullOrWhiteSpace(keyValue))
            {
                await _db.Cards.FirstOrDefaultAsync(c => c.Name == keyValue);
            }

            await next(context);
        }
    }
}
