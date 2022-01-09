using CMS.Enums;
using CMS.ResponseModels;
using Microsoft.AspNetCore.Mvc;

namespace CMS.Controllers
{
    public abstract class BaseController : ControllerBase
    {
        protected IActionResult Info()
        {
            return Ok(new ResponseModel()
            {
                Result = BusinessResult.Success                
            });
        }

        protected IActionResult Info<T>(T data)
        {
            return Ok(new ResponseDataModel<T>()
            {
                Data = data
            });
        }

        protected IActionResult Error(
            string message,
            BusinessResult result = BusinessResult.BasicError)
        {
            return BadRequest(new ResponseModel()
            {
                Result = result,
                Message = message
            });
        }
    }
}
