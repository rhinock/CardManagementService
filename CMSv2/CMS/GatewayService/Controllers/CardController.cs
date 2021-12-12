using Domain.Interfaces;
using Domain.Objects;
using GatewayService.Attributes;
using GatewayService.DataObjects;
using GatewayService.Enums;
using GatewayService.Models;
using GatewayService.ResponseModels;
using Microsoft.AspNetCore.Mvc;
using ObjectTools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GatewayService.Controllers
{
    /// <summary>
    /// Api controller for cards
    /// </summary>
    [Route("api/card")]
    public class CardController : BaseController
    {
        public CardController(ResourceConnection connection) : base(connection)
        {
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
        public async Task<ActionResult> GetCardById(Guid id)
        {
            Card card = await Repository.Get<Card>(c => c.Id == id);

            if (card == null)
                return await ErrorAsync("Card not found", BusinessResult.NotFound);

            return await InfoAsync(card.To<Card, CardModel>());
        }

        /// <summary>
        /// Get User cards
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Cards that belong to the user</returns>
        [HttpGet("user/{id:guid}")]
        [Logging]
        [ProducesResponseType(typeof(ResponseModel), 200)]
        public async Task<ActionResult> GetUserCards(Guid id)
        {
            IEnumerable<Card> cards = await Repository.GetMany<Card>(c => c.UserId == id);
            IEnumerable<CardModel> cardModels = cards.ToList().Select(c => c.To<Card, CardModel>());
            return await InfoAsync(cardModels);
        }

        /// <summary>
        /// Get cards
        /// </summary>
        /// <returns>List of cards</returns>
        [HttpGet]
        [Logging]
        [ProducesResponseType(typeof(ResponseModel), 200)]
        public async Task<ActionResult> GetCards()
        {
            //return Info(_repository
            //    .Query<Card>()
            //    .ToList()
            //    .Select(c => c.To<Card, CardModel>()));

            IEnumerable<Card> cards = await Repository.GetMany<Card>();
            IEnumerable<CardModel> cardModels = cards.ToList().Select(c => c.To<Card, CardModel>());
            return await InfoAsync(cardModels);
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
            //if (_repository.Query<Card>().Any(c => c.Id == model.Id))
            //    return await ErrorAsync("Id is already in use");

            Guid id = model.Id;
            IEnumerable<Card> cards = await Repository.GetMany<Card>(c => c.Id == id);

            if (cards.ToList().Any(c => c.Id != model.Id))
                return await ErrorAsync("Id is already in use");

            //if (_repository.Query<Card>().Any(c => c.Pan == model.Pan))
            //    return await ErrorAsync("Pan is already in use");

            if (cards.ToList().Any(c => c.Pan == model.Pan))
                return await ErrorAsync("Pan is already in use");

            Card card = model.To<CardModel, Card>();
            await Repository.Create(card);

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
        public async Task<ActionResult> EditCard(Guid id, [FromBody] CardEditModel model)
        {
            //var card = _repository
            //    .Query<Card>()
            //    .FirstOrDefault(c => c.Id == id);

            var card = await Repository.Get<Card>(c => c.Id == id);

            if (card == null)
                return await ErrorAsync("Card not found", BusinessResult.NotFound);

            card.Name = model.Name;
            await Repository.Update(card);

            return await InfoAsync();
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
        public async Task<ActionResult> DeleteCard(Guid id)
        {
            //var card = _repository
            //    .Query<Card>()
            //    .FirstOrDefault(c => c.Id == id);

            var card = await Repository.Get<Card>(c => c.Id == id);

            if (card == null)
                return await ErrorAsync("Card not found", BusinessResult.NotFound);

            await Repository.Delete(card);

            return await InfoAsync();
        }
    }
}
