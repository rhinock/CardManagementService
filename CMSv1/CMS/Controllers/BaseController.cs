using CMS.Enums;
using CMS.ResponseModels;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

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

        protected async Task<ActionResult> InfoAsync()
        {
            return await Task.FromResult(Ok(new ResponseModel()
            {
                Result = BusinessResult.Success
            }));
        }

        protected async Task<ActionResult> InfoAsync<T>(T data)
        {
            return await Task.FromResult(Ok(new ResponseDataModel<T>()
            {
                Data = data
            }));
        }

        protected async Task<ActionResult> ErrorAsync(
            string message,
            BusinessResult result = BusinessResult.BasicError)
        {
            return await Task.FromResult(BadRequest(new ResponseModel()
            {
                Result = result,
                Message = message
            }));
        }
    }
}
