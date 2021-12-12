
using System;
using System.Threading.Tasks;
using Domain.Interfaces;

using Domain.Objects;
using Infrastructure;
using Newtonsoft.Json;
using WebTools;

using Microsoft.AspNetCore.Http;
using LoggerService.Enums;

namespace LoggerService
{
    public class RequestHandling
    {
        private readonly RequestDelegate _next;

        public RequestHandling(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                string path = context.Request.Path.Value.ToLower();
                string method = context.Request.Method;
                string requestData = await context.Request.GetBodyAsStringAsync();
                Message message = JsonConvert.DeserializeObject<Message>(requestData);

                if(message?.Value == null)
                {
                    context.Response.StatusCode = 400;
                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsync(JsonConvert.SerializeObject(new { status = "Error", message = "Require \"Message\"" }));
                }
                else if(method != "POST")
                {
                    context.Response.StatusCode = 405;
                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsync(JsonConvert.SerializeObject(new { status = "Error" }));
                }
                else
                {
                    lock(AppContext.Logs)
                    {
                        switch (path)
                        {
                            case "/info":
                                AppContext.Logs.Add(new Log
                                {
                                    Content = message.Value,
                                    Type = LogType.Info
                                });
                                break;
                            case "/error":
                                AppContext.Logs.Add(new Log
                                {
                                    Content = message.Value,
                                    Type = LogType.Info
                                });
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
            }
            catch(Exception ex)
            {
                context.Response.StatusCode = 500;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(JsonConvert.SerializeObject(new { status = "Error", message = ex.ToString() }));
            }
        }

        private ILogger GetLogger(MiddlewareOptions options)
        {
            ILogger logger = LoggerManager.GetLogger(options.Get<ResourceConnection>("MainData"));
            return logger;
        }
    }
}