using CMS.Entities;
using CMS.Extensions;
using CMS.Models;
using CMS.Repositories;
using CMS.ResponseModels;
using CMS.Types;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.Controllers
{
    [Route("api/config")]
    [ApiController]
    public class ConfigController : BaseController
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<ConfigController> _logger;
        private readonly IRepository _repository;

        public ConfigController(
            IConfiguration configuration, 
            ILogger<ConfigController> logger,
            IRepository repository)
        {
            _logger = logger;
            _configuration = configuration;
            _repository = repository;
        }

        // TODO: use this method for filling data in DB
        /// <summary>
        /// Get configuration from json
        /// </summary>
        /// <returns>Collection of cards or empty array if there are no cards</returns>
        [HttpGet]
        [ProducesResponseType(typeof(ResponseModel), 200)]
        public async Task<ActionResult> GetConfigAsync()
        {
            _logger.LogInformation("ConfigController.GetConfig");

            if (!_repository.Query<Card>().Any())
            {
                var cardModels = _configuration.GetSection("Cards")?
                    .GetChildren()?
                    .Select(c => new CardModel()
                    {
                        Id = c.GetValue<Guid>("Id"),
                        Cvc = c.GetValue<string>("Cvc"),
                        Pan = c.GetValue<string>("Pan"),
                        Expire = new Expire(c.GetValue<int>("Expire:Month"), c.GetValue<int>("Expire:Year")),
                        Name = $"{_configuration["ASPNETCORE_ENVIRONMENT"]}_{c.GetValue<string>("Name")}",
                        IsDefault = c.GetValue<bool>("IsDefault"),
                        UserId = c.GetValue<Guid>("UserId")
                    })?
                    .ToList();

                foreach (var cardModel in cardModels)
                {
                    Card card = cardModel.To<CardModel, Card>();
                    await _repository.Create(card);
                }
            }

            return await InfoAsync(await _repository.Query<Card>().ToListAsync());
        }
    }
}
