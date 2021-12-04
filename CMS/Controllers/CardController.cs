using CMS.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Collections.Generic;
using CMS.Enums;
using CMS.Attributes;

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
        public ActionResult<List<Card>> GetCards()
        {
            return Ok(CardCollection.Cards);
        }

        /// <summary>
        /// Get a card by userId
        /// </summary>
        /// <param name="model"></param>
        /// <response code="200">OK</response>
        /// <response code="404">Card not found</response>
        /// <returns>A card that belongs to the user</returns>
        [HttpGet("Card")]
        [ProducesResponseType(typeof(Card), 200)]
        [ProducesResponseType(typeof(ResponseModel), 400)]
        [ProducesResponseType(typeof(ResponseModel), 404)]
        public IActionResult GetCard([FromQuery]GetCardRequestModel model)
        {
            CardResponseModel responseModel = new CardResponseModel();

            if (model.Id.HasValue)
            {
                responseModel.Items = CardCollection.Cards.Where(c =>c.Id == model.Id);
            }
            else if (model.UserId.HasValue)
            {
                responseModel.Items = CardCollection.Cards.Where(c => c.UserId == model.UserId);
            }
            else
            {
                responseModel.Items = CardCollection.Cards;
            }

            return Ok(responseModel);
        }

        /// <summary>
        /// Create a card
        /// </summary>
        /// <param name="card"></param>
        /// <response code="200">OK</response>
        /// <response code="400">Card model is not valid</response>
        /// <response code="409">Pan is already in use</response>
        /// <returns>A new card for the user</returns>
        //[ModelValidation]
        //[HttpPost("CreateCard")]
        //[ProducesResponseType(typeof(Card), 200)]
        //[ProducesResponseType(typeof(ApiError), 400)]
        //[ProducesResponseType(typeof(ApiError), 409)]
        //public IActionResult CreateCard([FromBody] Card card)
        //{
        //    if (CardCollection.Cards.Any(c => c.Id == card.Id))
        //        return Conflict(new ApiError()
        //        {
        //            Result = BusinessResult.Conflict,
        //            Message = "Id is already in use"
        //        });

        //    if (CardCollection.Cards.Any(c => c.Pan == card.Pan))
        //        return Conflict(new ApiError()
        //        {
        //            Result = BusinessResult.Conflict,
        //            Message = "Pan is already in use"
        //        });

        //    CardCollection.Cards.Add(card);

        //    return Ok(card);
        //}

        /// <summary>
        /// Edit a card
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <response code="200">OK</response>
        /// <response code="400">Card model is not valid</response>
        /// <response code="404">Card not found</response>
        /// <returns>Edited card with a new name</returns>
        //[ModelValidation]
        //[HttpPut("EditCard")]
        //[ProducesResponseType(typeof(Card), 200)]
        //[ProducesResponseType(typeof(BadRequestObjectResult), 400)]
        //[ProducesResponseType(typeof(ApiError), 404)]
        //public IActionResult EditCard([FromQuery] Guid id, [FromBody] string name)
        //{
        //    var card = CardCollection.Cards.FirstOrDefault(c => c.Id == id);

        //    if (card == null)
        //        return NotFound(new ApiError()
        //        {
        //            Result = BusinessResult.NotFound,
        //            Message = "Card not found"
        //        });

        //    card.Name = name;

        //    return Ok(card);
        //}

        /// <summary>
        /// Delete a card by id
        /// </summary>
        /// <param name="id"></param>
        /// <response code="200">OK</response>
        /// <response code="404">Card not found</response>
        /// <returns>OK if deletion is successful</returns>
        //[HttpDelete("DeleteCard")]
        //[ProducesResponseType(200)]
        //[ProducesResponseType(typeof(ApiError), 404)]
        //public IActionResult DeleteCard(Guid id)
        //{
        //    var card = CardCollection.Cards.FirstOrDefault(c => c.Id == id);

        //    if (card == null)
        //        return NotFound(new ApiError()
        //        {
        //            Result = BusinessResult.NotFound,
        //            Message = "Card not found"
        //        });

        //    CardCollection.Cards.Remove(card);

        //    return Ok();
        //}
    }
}
