using Infrastructure;

using Domain.Objects;
using Domain.Interfaces;

using GatewayService.Enums;
using GatewayService.ResponseModels;

using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace GatewayService.Controllers
{
    public abstract class BaseController : ControllerBase
    {
        private readonly IRepository _repository;
        private readonly ILogger _logger;

        public BaseController(Dictionary<string, ResourceConnection> connections)
        {
            _repository = connections["MainData"].Repository();
            _logger = connections["Logger"].Logger();
        }

        protected IRepository Repository => _repository;
        public ILogger Logger => _logger;

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
