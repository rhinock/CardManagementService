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
    [Route("api")]
    [ApiController]
    public class CardController : ControllerBase
    {
        /// <summary>
        /// Get all cards
        /// </summary>
        /// <returns>Collection of cards or empty array if there are no cards</returns>
        [HttpGet("cards")]
        [ProducesResponseType(typeof(ApiResponseModel<List<Card>>), 200)]
        public ActionResult<ApiResponseModel<List<Card>>> GetCards()
        {
            ApiResponseModel<List<Card>> responseModel = 
                new ApiResponseModel<List<Card>>(CardCollection.Cards);

            return Ok(responseModel);
        }

        /// <summary>
        /// Get cards by userId
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>Collection of cards that belong to the user</returns>
        [HttpGet("users/{userId:guid}/cards")]
        [ProducesResponseType(typeof(ApiResponseModel<List<Card>>), 200)]
        [ProducesResponseType(typeof(ApiResponseModel<List<Card>>), 400)]
        [ProducesResponseType(typeof(ApiResponseModel<List<Card>>), 404)]
        public ActionResult<ApiResponseModel<List<Card>>> GetCardsByUserId(Guid? userId)
        {
            ApiResponseModel<List<Card>> responseModel = 
                new ApiResponseModel<List<Card>>();

            if (!userId.HasValue)
            {
                responseModel.Result = BusinessResult.InvalidModel;
                responseModel.Message = "UserId shouldn't be null";
                return BadRequest(responseModel);
            }

            responseModel.Data = CardCollection.Cards
                .Where(c => c.UserId == userId)
                .ToList();

            if (responseModel.Data.Count == 0)
            {
                responseModel.Result = BusinessResult.NotFound;
                responseModel.Message = "User doesn't have cards";
                return NotFound(responseModel);
            }

            return Ok(responseModel);
        }

        /// <summary>
        /// Get card by Id
        /// </summary>
        /// <param name="cardId"></param>
        /// <returns>Card</returns>
        [HttpGet("cards/{cardId:guid}")]
        [ProducesResponseType(typeof(ApiResponseModel<Card>), 200)]
        [ProducesResponseType(typeof(ApiResponseModel<Card>), 400)]
        [ProducesResponseType(typeof(ApiResponseModel<Card>), 404)]
        public ActionResult<ApiResponseModel<Card>> GetCardById(Guid? cardId)
        {
            ApiResponseModel<Card> responseModel = 
                new ApiResponseModel<Card>();

            if (!cardId.HasValue)
            {
                responseModel.Result = BusinessResult.InvalidModel;
                responseModel.Message = "CardId shouldn't be null";
                return BadRequest(responseModel);
            }

            responseModel.Data = CardCollection.Cards
                .Where(c => c.Id == cardId)
                .FirstOrDefault();

            if (responseModel.Data == null)
            {
                responseModel.Result = BusinessResult.NotFound;
                responseModel.Message = "Card is't found";
                return NotFound(responseModel);
            }

            return Ok(responseModel);
        }

        /// <summary>
        /// Create a card
        /// </summary>
        /// <param name="card"></param>
        /// <returns>A new card for the specified user</returns>
        [ModelValidation]
        [HttpPost("cards")]
        [ProducesResponseType(typeof(ApiResponseModel<Card>), 200)]
        [ProducesResponseType(typeof(ApiResponseModel<Card>), 400)]
        [ProducesResponseType(typeof(ApiResponseModel<Card>), 409)]
        public ActionResult<ApiResponseModel<Card>> CreateCard([FromBody] Card card)
        {
            ApiResponseModel<Card> responseModel =
                new ApiResponseModel<Card>(card);

            if (CardCollection.Cards.Any(c => c.Id == card.Id))
            {
                responseModel.Result = BusinessResult.InvalidModel;
                responseModel.Message = "Id is already in use";
                return Conflict(responseModel);
            }

            if (CardCollection.Cards.Any(c => c.Pan == card.Pan))
            {
                responseModel.Result = BusinessResult.InvalidModel;
                responseModel.Message = "Pan is already in use";
                return Conflict(responseModel);
            }

            CardCollection.Cards.Add(card);
            responseModel.Data = card;

            return Ok(responseModel);
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
