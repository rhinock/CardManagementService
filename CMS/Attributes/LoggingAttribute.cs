using CMS.ResponseModels;
using log4net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Diagnostics;

namespace CMS.Attributes
{
    public class LoggingAttribute : ActionFilterAttribute
    {
        private readonly ILog _log = LogManager.GetLogger(typeof(LoggingAttribute));

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            foreach (var item in context.ActionArguments)
            {
                string value = item.Value?.ToString();

                _log.Info($"{context.HttpContext.Request.Path}");
                _log.Info($"{item.Key}: {value}");
            }
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Result is ObjectResult result)
            {
                string value = string.Empty;
                
                if (result.Value is ResponseModel responseModel)
                {
                    value = responseModel.ToString();
                }

                _log.Info($"{context.HttpContext.Request.Path}");
                _log.Info($"Response: {value}");
            }
        }
    }
}
