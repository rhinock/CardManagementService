using CMS.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using CMS.Enums;
using CMS.Attributes;
using CMS.ResponseModels;
using CMS.Extensions;
using CMS.Entities;
using CMS.Repositories;
using System.Threading.Tasks;

namespace CMS.Controllers
{
    /// <summary>
    /// Api controller for cards
    /// </summary>
    [Route("api/card")]
    public class CardController : BaseController
    {
        private readonly AppDbContext _appDbContext;
        private readonly IRepository _repository;

        public CardController(AppDbContext appDbContext, IRepository repository)
        {
            _appDbContext = appDbContext;
            _repository = repository;
        }

        /// <summary>
        /// Get a card by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id:guid}")]
        [Logging]
        [ProducesResponseType(typeof(ResponseModel), 200)]
        [ProducesResponseType(typeof(ResponseModel), 400)]
        public IActionResult GetCardById(Guid id)
        {
            Card card = _repository
                .Query<Card>()
                .FirstOrDefault(c => c.Id == id);

            if (card == null)
                return Error("Card not found", BusinessResult.NotFound);

            return Info(card.To<Card, CardModel>());
        }

        /// <summary>
        /// Get User cards
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Cards that belong to the user</returns>
        [HttpGet("user/{id:guid}")]
        [Logging]
        [ProducesResponseType(typeof(ResponseModel), 200)]
        public IActionResult GetUserCards(Guid id)
        {
            var cardModels = _repository
                .Query<Card>()
                .Where(c => c.UserId == id)
                .ToList()
                .Select(c => c.To<Card, CardModel>());

            return Info(cardModels);
        }

        /// <summary>
        /// Get cards
        /// </summary>
        /// <returns>List of cards</returns>
        [HttpGet]
        [Logging]
        [ProducesResponseType(typeof(ResponseModel), 200)]
        public IActionResult GetCards()
        {
            return Info(_repository
                .Query<Card>()
                .ToList()
                .Select(c => c.To<Card, CardModel>()));
        }

        /// <summary>
        /// Create a card
        /// </summary>
        /// <param name="model"></param>
        /// <returns>A new card for the user</returns>
        [Logging]
        [ModelValidation]
        [HttpPost]
        [ProducesResponseType(typeof(ResponseModel), 200)]
        [ProducesResponseType(typeof(ResponseModel), 400)]
        public async Task<ActionResult> CreateCard([FromBody] CardModel model)
        {
            // TODO: replace CardCollection.Cards to _repository

            if (_repository.Query<Card>().Any(c => c.Id == model.Id))
                return await ErrorAsync("Id is already in use");

            if (_repository.Query<Card>().Any(c => c.Pan == model.Pan))
                return await ErrorAsync("Pan is already in use");

            Card card = model.To<CardModel, Card>();
            await _repository.Create(card);

            return await InfoAsync(card);
        }

        /// <summary>
        /// Edit a card
        /// </summary>
        /// <returns></returns>
        [ModelValidation]
        [Logging]
        [HttpPut("{id:guid}")]
        [ProducesResponseType(typeof(ResponseModel), 200)]
        [ProducesResponseType(typeof(ResponseModel), 400)]
        public IActionResult EditCard(Guid id, [FromBody] CardEditModel model)
        {
            var card = _repository
                .Query<Card>()
                .FirstOrDefault(c => c.Id == id);

            if (card == null)
                return Error("Card not found", BusinessResult.NotFound);

            card.Name = model.Name;
            _repository.Update(card);

            return Info();
        }

        /// <summary>
        /// Delete a card by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Logging]
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(typeof(ResponseModel), 200)]
        [ProducesResponseType(typeof(ResponseModel), 400)]
        public IActionResult DeleteCard(Guid id)
        {
            var card = _repository
                .Query<Card>()
                .FirstOrDefault(c => c.Id == id);

            if (card == null)
                return Error("Card not found", BusinessResult.NotFound);

            _repository.Delete(card);

            return Info();
        }
    }
}