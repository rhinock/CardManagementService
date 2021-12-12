using WebTools;

using BalancerService.Objects;

using System.Threading.Tasks;

using Domain.Objects;
using Domain.Interfaces;

using Microsoft.AspNetCore.Http;

using Newtonsoft.Json;

using Infrastructure;

namespace BalancerService
{
    public class RequestHandling
    {
        private readonly RequestDelegate _next;
        private readonly MiddlewareOptions _options;
        private readonly IRepository _repository;

        public RequestHandling(RequestDelegate next, MiddlewareOptions options)
        {
            _next = next;
            _options = options;
            _repository = RepositoryManager.GetRepository(_options.Get<ResourceConnection>("MainData"));
        }

        public async Task InvokeAsync(HttpContext context)
        {
            string path = context.Request.Path.Value.ToLower().Replace("/", "");

            Route route = await _repository.Get<Route>(x => x.ObjectName == path);
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
