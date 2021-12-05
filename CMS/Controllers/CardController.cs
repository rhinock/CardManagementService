using CMS.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Collections.Generic;
using CMS.Enums;
using CMS.Attributes;
using CMS.ResponseModels;
using CMS.RequestModels;

namespace CMS.Controllers
{
    /// <summary>
    /// Api controller for cards
    /// </summary>
    [Route("api/card")]
    public class CardController : BaseController
    {
        /// <summary>
        /// Get a card by userId
        /// </summary>
        /// <param name="model"></param>
        /// <returns>A card that belongs to the user</returns>
        [HttpGet]
        [ProducesResponseType(typeof(ResponseModel), 200)]
        public IActionResult GetCard([FromQuery]GetCardRequestModel model)
        {
            IEnumerable<Card> cards;

            if (model.Id.HasValue)
            {
                cards = CardCollection.Cards.Where(c =>c.Id == model.Id);
            }
            else if (model.UserId.HasValue)
            {
                cards = CardCollection.Cards.Where(c => c.UserId == model.UserId);
            }
            else
            {
                cards = CardCollection.Cards;
            }

            return Info(cards);
        }

        /// <summary>
        /// Create a card
        /// </summary>
        /// <param name="card"></param>
        /// <returns>A new card for the user</returns>
        [ModelValidation]
        [HttpPost]
        [ProducesResponseType(typeof(ResponseModel), 200)]
        [ProducesResponseType(typeof(ResponseModel), 400)]
        public IActionResult CreateCard([FromBody] Card card)
        {
            if (CardCollection.Cards.Any(c => c.Id == card.Id))
                return Error("Id is already in use");

            if (CardCollection.Cards.Any(c => c.Pan == card.Pan))
                return Error("Pan is already in use");

            CardCollection.Cards.Add(card);

            return Info();
        }

        /// <summary>
        /// Edit a card
        /// </summary>
        /// <returns>Edited card with a new name</returns>
        [ModelValidation]
        [HttpPatch]
        [ProducesResponseType(typeof(ResponseModel), 200)]
        [ProducesResponseType(typeof(ResponseModel), 400)]
        public IActionResult EditCard([FromBody] EditCardRequestModel model)
        {
            var card = CardCollection.Cards.FirstOrDefault(c => c.Id == model.Id);

            if (card == null)
                return Error("Card not found", BusinessResult.NotFound);

            card.Name = model.Name;

            return Info();
        }

        /// <summary>
        /// Delete a card by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>OK if deletion is successful</returns>
        [HttpDelete]
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
