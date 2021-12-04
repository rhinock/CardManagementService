using CMS.Enums;
using CMS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CMS.Attributes
{
    public class ModelValidationAttribute : ActionFilterAttribute
    {
        /// <inheritdoc />
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
                context.Result = new BadRequestObjectResult(
                    // new ApiError
                    new ResponseModel
                    {
                        // Result = BusinessResult.BadRequest,
                        // Result = BusinessResult.Error,
                        Result= BusinessResult.InvalidModel,
                        Message = context.ModelState.Values.ToString()
                    });
        }
    }
}
