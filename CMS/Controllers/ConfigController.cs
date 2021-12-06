﻿using CMS.Entities;
using CMS.Models;
using CMS.Types;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CMS.Controllers
{
    [Route("Config")]
    [ApiController]
    public class ConfigController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<ConfigController> _logger;

        public ConfigController(IConfiguration configuration, ILogger<ConfigController> logger)
        {
            _logger = logger;
            _configuration = configuration;
        }

        /// <summary>
        /// Get configuration from json
        /// </summary>
        /// <response code="200">OK</response>
        /// <returns>Collection of cards or empty array if there are no cards</returns>
        [HttpGet("GetConfig")]
        [ProducesResponseType(typeof(List<Card>), 200)]
        public ActionResult<List<Card>> GetConfig()
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

            return Ok(CardCollection.Cards);
        }
    }
}
