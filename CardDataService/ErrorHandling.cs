using System;
using System.Threading.Tasks;

using Infrastructure;

using Domain.Objects;

using WebTools;
using WebTools.Middlewares;

using Newtonsoft.Json;

using Microsoft.AspNetCore.Http;

namespace CardDataService
{
    public class ErrorHandling : ExceptionHandlingMiddleware
    {
        public ErrorHandling(RequestDelegate next, MiddlewareOptions options = null)
            : base(next, options) { }

        protected override async Task OnError(HttpContext context, Exception ex)
        {
            context.Response.StatusCode = 500;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(JsonConvert.SerializeObject(
                new
                {

                    message = ex.ToString(),
                    innerException = ex.InnerException?.ToString()
                }));

            await Options.Get<ResourceConnection>("Logger").Logger().Error(ex.ToString());
        }
    }
}