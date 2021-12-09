using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebTools.Middlewares
{
    public abstract class AuthenticationMiddleware
    {
        private RequestDelegate _requestDelegate;
        public AuthenticationMiddleware(RequestDelegate requestDelegate)
        {
            _requestDelegate = requestDelegate;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            string authorizationValue = context.Request.Headers["Authorization"];

            if (Auth(authorizationValue))
            {
                await _requestDelegate.Invoke(context);
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
