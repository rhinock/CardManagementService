using CMS.Models;
using CMS.Filters;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Collections.Generic;
using CMS.Enums;
using Newtonsoft.Json;

namespace CMS.Controllers
{
    /// <summary>
    /// Api controller for cards
    /// </summary>
    [Route("Card")]
    [ApiController]
    public class CardController : ControllerBase
    {
        /// <summary>
        /// Get all cards
        /// </summary>
        /// <response code="200">OK</response>
        /// <returns>Collection of cards or empty array if there are no cards</returns>
        [HttpGet("GetCards")]
        [ProducesResponseType(typeof(List<Card>), 200)]
        public IActionResult GetCards()
        {
            var json = JsonConvert.SerializeObject(CardCollection.Cards, Formatting.Indented);
            return Ok(json);
        }

        /// <summary>
        /// Get a card by userId
        /// </summary>
        /// <param name="userId"></param>
        /// <response code="200">OK</response>
        /// <response code="404">Card not found</response>
        /// <returns>A card that belongs to the user</returns>
        [HttpGet]
        [ProducesResponseType(typeof(Card), 200)]
        [ProducesResponseType(typeof(ApiError), 404)]
        public IActionResult GetCard(Guid userId)
        {
            var cards = CardCollection.Cards.Where(c => c.UserId == userId);

            if (cards == null)
                return NotFound(new ApiError() 
                { 
                    Result = BusinessResult.NotFound, 
                    Message = "Card not found" 
                });

            var json = JsonConvert.SerializeObject(cards, Formatting.Indented);
            return Ok(json);
        }

        /// <summary>
        /// Create a card
        /// </summary>
        /// <param name="card"></param>
        /// <response code="200">OK</response>
        /// <response code="400">Card model is not valid</response>
        /// <response code="409">Pan is already in use</response>
        /// <returns>A new card for the user</returns>
        [ModelValidation]
        [HttpPost]
        [ProducesResponseType(typeof(Card), 200)]
        [ProducesResponseType(typeof(ApiError), 400)]
        [ProducesResponseType(typeof(ApiError), 409)]
        public IActionResult CreateCard([FromBody] Card card)
        {
            if (!CardCollection.Cards.Any(c => c.Pan == card.Pan))
                CardCollection.Cards.Add(card);
            else
                return Conflict(new ApiError() 
                { 
                    Result = BusinessResult.Conflict,
                    Message = "Pan is already in use"
                });

            var json = JsonConvert.SerializeObject(card, Formatting.Indented);
            return Ok(json);
        }

        /// <summary>
        /// Edit a card
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <response code="200">OK</response>
        /// <response code="400">Card model is not valid</response>
        /// <response code="404">Card not found</response>
        /// <returns>Edited card with a new name</returns>
        [ModelValidation]
        [HttpPut]
        [ProducesResponseType(typeof(Card), 200)]
        [ProducesResponseType(typeof(BadRequestObjectResult), 400)]
        [ProducesResponseType(typeof(ApiError), 404)]
        public IActionResult EditCard([FromRoute] Guid id, [FromBody] string name)
        {
            var card = CardCollection.Cards.FirstOrDefault(c => c.Id == id);

            if (card == null)
                return NotFound(new ApiError()
                {
                    Result = BusinessResult.NotFound,
                    Message = "Card not found"
                });

            card.Name = name;

            var json = JsonConvert.SerializeObject(card, Formatting.Indented);
            return Ok(json);
        }

        /// <summary>
        /// Delete a card by id
        /// </summary>
        /// <param name="id"></param>
        /// <response code="200">OK</response>
        /// <response code="404">Card not found</response>
        /// <returns>OK if deletion is successful</returns>
        [HttpDelete]
        [ProducesResponseType(typeof(OkResult), 200)]
        [ProducesResponseType(typeof(ApiError), 404)]
        public IActionResult DeleteCard(Guid id)
        {
            var card = CardCollection.Cards.FirstOrDefault(c => c.Id == id);

            if (card == null)
                return NotFound(new ApiError()
                {
                    Result = BusinessResult.NotFound,
                    Message = "Card not found"
                });

            CardCollection.Cards.Remove(card);

            return Ok();
        }
    }
}
