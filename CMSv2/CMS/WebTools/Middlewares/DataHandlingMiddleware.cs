using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace WebTools.Middlewares
{
    public abstract class DataHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly MiddlewareOptions _options;

        public DataHandlingMiddleware(RequestDelegate next, MiddlewareOptions options = null)
        {
            _next = next;
            _options = options;
        }

        protected MiddlewareOptions Options => _options;

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                string method = context.Request.Method;
                string path = context.Request.Path.Value.ToLower();

                string prefix = _options.Get<string>("Prefix");

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
            catch(Exception ex)
            {
                await OnError(context, ex);
            }
        }

        protected abstract Task OnGet(HttpContext context);
        protected abstract Task OnPost(HttpContext context);
        protected abstract Task OnPatch(HttpContext context);
        protected abstract Task OnDelete(HttpContext context);

        protected virtual async Task OnError(HttpContext context, Exception ex)
        {
            context.Response.StatusCode = 500;
            await context.Response.WriteAsync(ex.Message);
        }


        protected async Task<string> GetBodyContent(HttpContext context)
        {
            string body;

            using (StreamReader reader = new StreamReader(context.Request.Body, Encoding.UTF8))
            {
                body = await reader.ReadToEndAsync();
            }

            return body;
        }

        protected async Task SetResponseObject<T>(HttpContext context, T data)
        {
            context.Response.StatusCode = 200;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(JsonConvert.SerializeObject(data));
        }
    }
}
