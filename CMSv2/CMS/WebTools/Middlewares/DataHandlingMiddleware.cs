using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

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
            string method = context.Request.Method;
            string path = context.Request.Path.Value.ToLower();

            string prefix = _options.Get<string>("Prefix");

            if(path.StartsWith($"/{prefix}"))
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
                    default:
                        context.Response.StatusCode = 404;
                        break;
                }
            }
            else
            {
                context.Response.StatusCode = 404;
            }
        }

        public abstract Task OnGet(HttpContext context);
        public abstract Task OnPost(HttpContext context);
        public abstract Task OnPatch(HttpContext context);

        public virtual void OnError(HttpContext context)
        {
            context.Response.StatusCode = 401;
        }
    }
}
