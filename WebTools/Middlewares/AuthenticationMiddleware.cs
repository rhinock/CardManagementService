using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;

namespace WebTools.Middlewares
{
    public abstract class AuthenticationMiddleware : BaseMiddleware
    {
        public AuthenticationMiddleware(RequestDelegate next, MiddlewareOptions options = null) 
            : base(next, options) { }

        public override async Task InvokeAsync(HttpContext context)
        {
            string authorizationValue = context.Request.Headers["Authorization"];

            if (await Auth(authorizationValue))
            {
                await Next.Invoke(context);
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
