using CMS.ResponseModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Diagnostics;

namespace CMS.Attributes
{
    public class LoggingAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            foreach (var item in context.ActionArguments)
            {
                string value = item.Value?.ToString();

                Debug.WriteLine($"{context.HttpContext.Request.Path}");
                Debug.WriteLine($"{item.Key}: {value}");
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

                Debug.WriteLine($"{context.HttpContext.Request.Path}");
                Debug.WriteLine($"Response: {value}");
            }
        }
    }
}
