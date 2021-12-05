using CMS.Enums;
using System.Linq;
using CMS.Models;
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
                    // new ApiError
                    new ResponseModel
                    {
                        // Result = BusinessResult.BadRequest,
                        // Result = BusinessResult.Error,
                        Result = BusinessResult.InvalidModel,
                        Message = context.ModelState.Values.First().Errors.First().ErrorMessage
                    });
        }

        //public override void OnResultExecuted(ResultExecutedContext context)
        //{
        //    base.OnResultExecuted(context);

        //    if (!context.ModelState.IsValid)
        //        context.Result = new BadRequestObjectResult(
        //            // new ApiError
        //            new ResponseModel
        //            {
        //                    // Result = BusinessResult.BadRequest,
        //                    // Result = BusinessResult.Error,
        //                    Result = BusinessResult.InvalidModel,
        //                Message = context.ModelState.Values.ToString()
        //            });
        //}

        //public override void OnResultExecuting(ResultExecutingContext context)
        //{
        //    base.OnResultExecuting(context);

        //    if (!context.ModelState.IsValid)
        //        context.Result = new BadRequestObjectResult(
        //            // new ApiError
        //            new ResponseModel
        //            {
        //                // Result = BusinessResult.BadRequest,
        //                // Result = BusinessResult.Error,
        //                Result = BusinessResult.InvalidModel,
        //                Message = context.ModelState.Values.ToString()
        //            });
        //}
    }
}
