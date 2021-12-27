using System;
using System.Threading.Tasks;

using WebTools;
using WebTools.Middlewares;

using Domain.Objects;
using Domain.Interfaces;

using Infrastructure;

using Newtonsoft.Json;

using Microsoft.AspNetCore.Http;

namespace LoggerService
{
    public class RequestHandling : BaseMiddleware
    {
        private readonly ILogger _logger;

        public RequestHandling(RequestDelegate next, MiddlewareOptions options) : base(next, options)
        {
            _logger = GetLogger(options);
        }

        public override async Task InvokeAsync(HttpContext context)
        {
            try
            {
                string path = context.Request.Path.Value.ToLower();
                string method = context.Request.Method;
                string requestData = await context.Request.GetBodyAsStringAsync();
                Message message = JsonConvert.DeserializeObject<Message>(requestData);

                if (message?.Value == null)
                {
                    context.Response.StatusCode = 400;
                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsync(JsonConvert.SerializeObject(new { status = "Error", message = "Require \"Message\"" }));
                    return;
                }
                else if (method != "POST")
                {
                    context.Response.StatusCode = 405;
                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsync(JsonConvert.SerializeObject(new { status = "Error" }));
                    return;
                }
                else
                {
                    switch (path)
                    {
                        case "/info":
                            await _logger.Info(message.Value);
                            break;
                        case "/error":
                            await _logger.Error(message.Value);
                            break;
                        default:
                            context.Response.StatusCode = 404;
                            return;
                    }
                }

                context.Response.StatusCode = 200;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(JsonConvert.SerializeObject(new { status = "OK" }));
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = 500;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(JsonConvert.SerializeObject(new
                {
                    status = "Error",
                    message = ex.ToString()
                }));
            }
        }

        private ILogger GetLogger(MiddlewareOptions options)
        {
            ILogger logger = LoggerManager.GetLogger(options.Get<ResourceConnection>("MainData"));
            return logger;
        }
    }
}