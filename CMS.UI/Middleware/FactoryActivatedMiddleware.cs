using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using CMS.UI.Data;

namespace CMS.UI.Middleware
{
    public class FactoryActivatedMiddleware : IMiddleware
    {
        private readonly ApplicationDbContext _db;

        public FactoryActivatedMiddleware(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            if (context.Request.Method == "GET" &&
                context.Request.Query.TryGetValue("cardName", out var cardName))
            {
                var card = await _db.Cards.FirstOrDefaultAsync(c => c.Name == cardName);
                await context.Response.WriteAsJsonAsync(card);
                return;
            }

            var keyValue = context.Request.Query["FactoryActivatedMiddleware"];

            if (!string.IsNullOrWhiteSpace(keyValue))
            {
                await _db.Cards.FirstOrDefaultAsync(c => c.Name == keyValue);
            }

            await next(context);
        }
    }
}
