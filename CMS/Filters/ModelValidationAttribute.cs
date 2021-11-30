using CMS.Enums;
using CMS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CMS.Filters
{
    public class ModelValidationAttribute : ActionFilterAttribute
    {
        /// <inheritdoc />
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
                context.Result = new BadRequestObjectResult(
                    new ApiError
                    {
                        Result = BusinessResult.BadRequest,
                        Message = context.ModelState.Values.ToString()
                    });
        }
    }
}
