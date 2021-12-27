using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;

namespace WebTools.Middlewares
{
    public abstract class BaseMiddleware
    {
        protected readonly RequestDelegate Next;

        public BaseMiddleware(RequestDelegate next, MiddlewareOptions options)
        {
            Next = next;
            Options = options;
        }

        public MiddlewareOptions Options { get; }

        public abstract Task InvokeAsync(HttpContext context);
    }
}