using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Linq;
using System.Threading.Tasks;
using WebApplication8.Data;
using WebApplication8.Models;

namespace WebApplication8.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CardController : ControllerBase
    {
        private ApplicationDbContext _dbContext;

        // 1. inject IOptions
        // 2. make the configuration values available as a property
        private Card _configs;
        public CardController(IOptions<Card> opts, ApplicationDbContext dbContext)
        {
            _configs = opts.Value;
            _dbContext = dbContext;
        }

        [HttpGet("GetConfig")]
        public IActionResult GetConfig()
        {
            // 3. return the config to inspect it
            var configs = _configs;
            return Ok(new { Result = "config values: ", configs });
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var cards = await _dbContext.Cards.ToListAsync();
            return Ok(cards);
        }

        //public IActionResult Get()
        //{
        //    return Ok();
        //}
    }
}
