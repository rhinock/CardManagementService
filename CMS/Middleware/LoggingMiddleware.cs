﻿using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace CMS.Middleware
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<LoggingMiddleware> _logger;

        public LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            _logger.LogInformation(
                $"Headers: {context.Request?.Headers}\n" +
                $"Query: {context.Request?.Query}\n" +
                $"Body: {context.Request?.Body}\n" +
                $"ContentType: {context.Request?.ContentType}");

            await _next(context);
        }
    }
}
