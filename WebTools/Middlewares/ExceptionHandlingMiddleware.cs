using System;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;

namespace WebTools.Middlewares
{
    public abstract class ExceptionHandlingMiddleware : BaseMiddleware
    {
        public ExceptionHandlingMiddleware(RequestDelegate next, MiddlewareOptions options = null) 
            : base(next, options) { }

        public override async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await Next(context);
            }
            catch (Exception ex)
            {
                await OnError(context, ex);
            }
        }

        protected abstract Task OnError(HttpContext context, Exception ex);
    }
}