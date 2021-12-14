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

        public MiddlewareOptions Options => _options;

        public async Task InvokeAsync(HttpContext context)
        {
            string authorizationValue = context.Request.Headers["Authorization"];

            if (await Auth(authorizationValue))
            {
                await _next.Invoke(context);
            }
            else
            {
                OnError(context);
            }
        }

        public abstract Task<bool> Auth(string authorizationValue);

        public virtual void OnError(HttpContext context)
        {
            context.Response.StatusCode = 401;
        }
    }
}
