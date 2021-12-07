using CMS.Entities;
using CMS.Extensions;
using CMS.Models;
using CMS.ResponseModels;
using CMS.Types;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace CMS.Controllers
{
    [Route("api/config")]
    [ApiController]
    public class ConfigController : BaseController
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<ConfigController> _logger;

        public ConfigController(IConfiguration configuration, ILogger<ConfigController> logger)
        {
            _logger = logger;
            _configuration = configuration;
        }

        // TODO: use this method for filling data in DB
        /// <summary>
        /// Get configuration from json
        /// </summary>
        /// <returns>Collection of cards or empty array if there are no cards</returns>
        [HttpGet]
        [ProducesResponseType(typeof(ResponseModel), 200)]
        public IActionResult GetConfig()
        {
            _logger.LogInformation("ConfigController.GetConfig");

            if (CardCollection.Cards.Count == 0)
            {
                CardCollection.Cards = _configuration.GetSection("Cards")
                        .GetChildren()
                        .Select(c => new Card()
                        {
                            Id = c.GetValue<Guid>("Id"),
                            Cvc = c.GetValue<string>("Cvc"),
                            Pan = c.GetValue<string>("Pan"),
                            Expire = new Expire(c.GetValue<int>("Expire:Month"), c.GetValue<int>("Expire:Year")),
                            Name = $"{_configuration["ASPNETCORE_ENVIRONMENT"]}_{c.GetValue<string>("Name")}",
                            IsDefault = c.GetValue<bool>("IsDefault"),
                            UserId = c.GetValue<Guid>("UserId")
                        })
                        .ToList<Card>();
            }

            return Info(CardCollection.Cards.Select(c => c.To<Card, CardModel>()));
        }
    }
}
