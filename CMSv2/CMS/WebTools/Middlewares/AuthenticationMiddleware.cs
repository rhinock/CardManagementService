using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace WebTools.Middlewares
{
    public abstract class AuthenticationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly MiddlewareOptions _options;

        public AuthenticationMiddleware(RequestDelegate next, MiddlewareOptions options = null)
        {
            _next = next;
            _options = options;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            string authorizationValue = context.Request.Headers["Authorization"];

            if (Auth(authorizationValue))
            {
                await _next.Invoke(context);
            }
            else
            {
                OnError(context);
            }
        }

        public abstract bool Auth(string authorizationValue);

        public virtual void OnError(HttpContext context)
        {
            context.Response.StatusCode = 401;
        }
    }
}
