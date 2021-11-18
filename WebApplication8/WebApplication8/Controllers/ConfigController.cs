using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using WebApplication8.Config;

namespace WebApplication8.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConfigController : ControllerBase
    {
        // 1. inject IOptions
        // 2. make the configuration values available as a property
        private MyConfig _configs;
        public ConfigController(IOptions<MyConfig> opts)
        {
            _configs = opts.Value;
        }

        [HttpGet("GetConfig")]
        public IActionResult GetConfig()
        {
            // 3. return the config to inspect it
            var configs = _configs;
            return Ok(new { Result = "config values: ", configs });
        }
    }
}
