using CMS.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CMS.Controllers
{
    public class ConfigController : Controller
    {
        private Card _configs;
        private readonly ILogger<ConfigController> _logger;

        public ConfigController(IOptions<Card> opts, ILogger<ConfigController> logger)
        {
            _configs = opts.Value;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult GetConfig()
        {
            _logger.LogInformation("ConfigController.GetConfig");
            return Ok(_configs);
        }
    }
}
