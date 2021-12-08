using CMS.Enums;
using System.Linq;
using CMS.ResponseModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using CMS.Models;
using System.Reflection;
using CMS.Extensions;

namespace CMS.Attributes
{
    public class ModelValidationAttribute : ActionFilterAttribute
    {
        /// <inheritdoc />
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            if (!context.ModelState.IsValid)
            {
                context.Result = new BadRequestObjectResult(
                    new ResponseModel
                    {
                        Result = BusinessResult.InvalidModel,
                        Message = context.ModelState.GetErrorMessage()
                    });
            }
            else
            {
                foreach (var actionArgument in context.ActionArguments)
                {
                    if (actionArgument.Value is Model)
                    {
                        var properties = actionArgument.Value
                            .GetType()
                            .GetProperties()
                            .Where(x => x.GetCustomAttribute<ValidateMemberAttribute>() != null);

                        foreach (var propertyInfo in properties)
                        {
                            var propertyInfoValue = propertyInfo.GetValue(actionArgument.Value);

                            if (propertyInfoValue != null)
                            {
                                bool isValid = (context.Controller as ControllerBase)
                                    .TryValidateModel(propertyInfoValue, $"{propertyInfo.Name}.");

                                if (!isValid)
                                {
                                    context.Result = new BadRequestObjectResult(
                                        new ResponseModel
                                        {
                                            Result = BusinessResult.InvalidModel,
                                            Message = context.ModelState.GetErrorMessage()
                                        });
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
