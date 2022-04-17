using System;
using System.Collections.Generic;

using Domain.Objects;
using Domain.Interfaces;

using Infrastructure;

using GatewayService.ResponseModels;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Threading.Tasks;

namespace GatewayService.Attributes
{
    public class LoggingAttribute : Attribute, IAsyncActionFilter
    {
        private readonly ILogger _logger;

        public LoggingAttribute(Dictionary<string, ResourceConnection> connections)
        {
            _logger = connections["Logger"].Logger();
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            string @params = string.Empty;
            foreach (var item in context.ActionArguments)
            {
                string value = item.Value?.ToString();
                @params = $"{@params}, {item.Key}={value}";
            }
            @params = @params.Trim(',').Trim();

            var resultContext = await next();
            string response = string.Empty;

            if (resultContext.Result is ObjectResult result)
            {
                if (result.Value is ResponseModel responseModel)
                {
                    response = responseModel.ToString();
                }
            }

            string message =
                $"Path: {context.HttpContext.Request.Path}; " +
                $"Method: {context.HttpContext.Request.Method}; " +
                $"Params: {@params}; " +
                $"Response: {response}";

            await _logger.Info(message);
        }
    }
}
