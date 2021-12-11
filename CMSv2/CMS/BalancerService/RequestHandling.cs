using WebTools;

using BalancerService.Objects;

using System.Threading.Tasks;

using Domain.Interfaces;

using Microsoft.AspNetCore.Http;

using Newtonsoft.Json;

namespace BalancerService
{
    public class RequestHandling
    {
        private readonly RequestDelegate _next;
        private readonly MiddlewareOptions _options;

        public RequestHandling(RequestDelegate next, MiddlewareOptions options = null)
        {
            _next = next;
            _options = options;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            string path = context.Request.Path.Value.ToLower().Replace("/", "");
            IRepository repository = _options.Get<IRepository>("Repository");

            Route route = repository.Get<Route>(x => x.ObjectName == path);
            if(route == null)
            {
                context.Response.StatusCode = 404;
            }
            else
            {
                context.Response.StatusCode = 200;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(JsonConvert.SerializeObject(route));
            }
        }
    }
}
