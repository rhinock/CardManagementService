using CMS.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using CMS.Enums;
using CMS.Attributes;
using CMS.ResponseModels;
using CMS.Extensions;
using CMS.Entities;

namespace CMS.Controllers
{
    /// <summary>
    /// Api controller for cards
    /// </summary>
    [Route("api/card")]
    public class CardController : BaseController
    {
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
            Card card = CardCollection.Cards.FirstOrDefault(c => c.Id == id);

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
            var cardModels = CardCollection.Cards
                .Where(c => c.UserId == id)
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
            return Info(CardCollection.Cards.Select(c => c.To<Card, CardModel>()));
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
        public IActionResult CreateCard([FromBody] CardModel model)
        {
            if (CardCollection.Cards.Any(c => c.Id == model.Id))
                return Error("Id is already in use");

            if (CardCollection.Cards.Any(c => c.Pan == model.Pan))
                return Error("Pan is already in use");

            Card card = model.To<CardModel, Card>();

            CardCollection.Cards.Add(card);

            return Info(card.Id);
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
            var card = CardCollection.Cards.FirstOrDefault(c => c.Id == id);

            if (card == null)
                return Error("Card not found", BusinessResult.NotFound);

            card.Name = model.Name;

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
            var card = CardCollection.Cards.FirstOrDefault(c => c.Id == id);

            if (card == null)
                return Error("Card not found", BusinessResult.NotFound);

            CardCollection.Cards.Remove(card);

            return Info();
        }
    }
}