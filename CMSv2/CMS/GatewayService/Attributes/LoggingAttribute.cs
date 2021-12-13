using Domain.Interfaces;

using GatewayService.Controllers;
using GatewayService.ResponseModels;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace GatewayService.Attributes
{
    public class LoggingAttribute : ActionFilterAttribute
    {

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            string message = string.Empty;
            foreach (var item in context.ActionArguments)
            {
                string value = item.Value?.ToString();
                message = $"{message}, {item}={value}";
            }

            message = $"Path: {context.HttpContext.Request.Path}; Request ID: {context.HttpContext.TraceIdentifier}; Method: {context.HttpContext.Request.Method}; Params: {message}";
            GetLogger(context).Info(message);
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

                message = $"Path: {context.HttpContext.Request.Path}; Request ID: {context.HttpContext.TraceIdentifier}; Method: {context.HttpContext.Request.Method}; Response: {message}";
                GetLogger(context).Info(message);
            }
        }

        private ILogger GetLogger(FilterContext context)
        {
            BaseController thisController = null;

            if(context is ActionExecutingContext actionExecutingContext)
            {
                if(actionExecutingContext.Controller is BaseController)
                {
                    thisController = (BaseController)actionExecutingContext.Controller;
                }
            }
            else if(context is ActionExecutedContext actionExecutedContext)
            {
                if (actionExecutedContext.Controller is BaseController)
                {
                    thisController = (BaseController)actionExecutedContext.Controller;
                }
            }

            if(thisController != null)
            {
                return thisController.Logger;
            }
            else
            {
                return null;
            }
        }
    }
}
