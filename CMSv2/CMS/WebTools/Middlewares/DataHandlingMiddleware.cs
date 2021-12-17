using System;
using System.IO;
using System.Threading.Tasks;

using Newtonsoft.Json;

using Microsoft.AspNetCore.Http;

namespace WebTools.Middlewares
{
    public abstract class DataHandlingMiddleware : BaseMiddleware
    {
        public DataHandlingMiddleware(RequestDelegate next, MiddlewareOptions options = null)
            : base(next, options) { }

        public override async Task InvokeAsync(HttpContext context)
        {
            string method = context.Request.Method;
            string path = context.Request.Path.Value.ToLower();

            string prefix = Options.Get<string>("Prefix");

            if (path.StartsWith($"/{prefix}"))
            {
                switch (method)
                {
                    case "GET":
                        await OnGet(context);
                        break;
                    case "POST":
                        await OnPost(context);
                        break;
                    case "PATCH":
                        await OnPatch(context);
                        break;
                    case "DELETE":
                        await OnDelete(context);
                        break;
                    default:
                        context.Response.StatusCode = 405;
                        break;
                }
            }
            else
            {
                context.Response.StatusCode = 404;
            }
        }

        protected abstract Task OnGet(HttpContext context);
        protected abstract Task OnPost(HttpContext context);
        protected abstract Task OnPatch(HttpContext context);
        protected abstract Task OnDelete(HttpContext context);

        protected async Task SetResponseObject<T>(HttpContext context, T data)
        {
            context.Response.StatusCode = 200;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(JsonConvert.SerializeObject(data));
        }
    }
}
