using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using WebTools;
using WebTools.Middlewares;

using Infrastructure;

using Domain.Objects;

using Newtonsoft.Json;

using GatewayService.Enums;
using GatewayService.ResponseModels;

using Microsoft.AspNetCore.Http;

namespace GatewayService
{
    public class ErrorHandling : ExceptionHandlingMiddleware
    {
        public ErrorHandling(RequestDelegate next, MiddlewareOptions options = null)
            : base(next, options) { }

        protected override async Task OnError(HttpContext context, Exception ex)
        {
            context.Response.StatusCode = 500;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(JsonConvert.SerializeObject(new ResponseModel
            {
                Result = BusinessResult.BasicError,
                Message = "Server error"
            }));

            string message;

            if (ex is WebException webEx)
            {
                string response = "";
                if (webEx?.Response?.ContentLength > 0)
                {
                    using (StreamReader reader = new StreamReader(webEx.Response.GetResponseStream(), Encoding.UTF8))
                    {
                        response = await reader.ReadToEndAsync();
                    }
                }
                message = $"{webEx.Message}, {response}";
            }
            else
            {
                message = ex.ToString();
            }

            await Options.Get<ResourceConnection>("Logger").Logger().Error(message);
        }
    }
}
