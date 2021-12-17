using System;
using System.Collections.Generic;

using Domain.Objects;
using Domain.Interfaces;

using Infrastructure;

using GatewayService.ResponseModels;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace GatewayService.Attributes
{
    public class LoggingAttribute : ActionFilterAttribute
    {
        private readonly ILogger _logger;

        public LoggingAttribute(Dictionary<string, ResourceConnection> connections)
        {
            _logger = connections["Logger"].Logger();
    }

    public override void OnActionExecuting(ActionExecutingContext context)
        {
            string message = string.Empty;
            foreach (var item in context.ActionArguments)
            {
                string value = item.Value?.ToString();
                message = $"{message}, {item}={value}";
            }

            string requestId = Guid.NewGuid().ToString("N").ToLower();
            context.HttpContext.Request.Headers.Add("X-Request-Id", requestId);

            message = $"Path: {context.HttpContext.Request.Path}; Request ID: {requestId}; Method: {context.HttpContext.Request.Method}; Params: {message}";
            _logger.Info(message);
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Result is ObjectResult result)
            {
                string message = string.Empty;
                
                if (result.Value is ResponseModel responseModel)
                {
                    message = responseModel.ToString();
                }

                message = $"Path: {context.HttpContext.Request.Path}; Request ID: {context.HttpContext.Request.Headers["X-Request-Id"]}; Method: {context.HttpContext.Request.Method}; Response: {message}";
                _logger.Info(message);
            }
        }
    }
}
