using CMS.Enums;
using System.Linq;
using CMS.ResponseModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CMS.Attributes
{
    public class ModelValidationAttribute : ActionFilterAttribute
    {
        /// <inheritdoc />
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            if (!context.ModelState.IsValid)
                context.Result = new BadRequestObjectResult(
                    new ResponseModel
                    {
                        Result = BusinessResult.InvalidModel,
                        Message = context.ModelState
                            .Select(x => x.Value.Errors)
                            .Where(x => x.Count > 0)
                            .FirstOrDefault()?
                            .FirstOrDefault()?
                            .ErrorMessage
                    });
        }
    }
}
