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
                    new ApiResponseModel<Card>
                    {
                        // Result = BusinessResult.BadRequest,
                        // Result = BusinessResult.Error,
                        ErrorCode= BusinessResult.InvalidModel,
                        ErrorMessage = context.ModelState.Values.ToString()
                    });
        }

        //public override void OnActionExecuted(ActionExecutedContext context)
        //{
        //    base.OnActionExecuted(context);
        //}

        public override void OnResultExecuting(ResultExecutingContext context)
        {
            base.OnResultExecuting(context);

            if (!context.ModelState.IsValid)
            {
                context.Result = new BadRequestObjectResult(
                    new ApiResponseModel<Card>()
                    {
                        ErrorCode = BusinessResult.InvalidModel,
                        ErrorMessage = context.ModelState.Values.ToString()
                    });
            }
        }
    }
}
