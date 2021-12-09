using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebTools.Middlewares
{
    public abstract class DataHandlingMiddleware
    {
        private RequestDelegate _requestDelegate;
        public DataHandlingMiddleware(RequestDelegate requestDelegate)
        {
            _requestDelegate = requestDelegate;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            string method = context.Request.Method;

            switch (method)
            {
                case "GET":
                    OnGet(context);
                    break;
                case "POST":
                    OnPost(context);
                    break;
                case "PATCH":
                    OnPatch(context);
                    break;
                default:
                    context.Response.StatusCode = 404;
                    break;
            }


        }

        public abstract bool OnGet(HttpContext context);
        public abstract bool OnPost(HttpContext context);
        public abstract bool OnPatch(HttpContext context);

        public virtual void OnError(HttpContext context)
        {
            context.Response.StatusCode = 401;
        }
    }
}
