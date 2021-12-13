
using System;
using System.Threading.Tasks;

using WebTools;

using Domain.Objects;
using Domain.Interfaces;

using Infrastructure;

using Newtonsoft.Json;

using Microsoft.AspNetCore.Http;

namespace LoggerService
{
    public class RequestHandling
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public RequestHandling(RequestDelegate next, MiddlewareOptions options)
        {
            _next = next;
            _logger = GetLogger(options);
        }

        public async Task InvokeAsync(HttpContext context)
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
                }
                else if (method != "POST")
                {
                    context.Response.StatusCode = 405;
                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsync(JsonConvert.SerializeObject(new { status = "Error" }));
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