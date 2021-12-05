using CMS.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Collections.Generic;
using CMS.Enums;
using CMS.Attributes;
using System.ComponentModel.DataAnnotations;

namespace CMS.Controllers
{
    /// <summary>
    /// Api controller for cards
    /// </summary>
    [Route("api/cards")]
    [ApiController]
    public class CardController : ControllerBase
    {
        /// <summary>
        /// Get all cards
        /// </summary>
        /// <returns>Collection of cards or empty array if there are no cards</returns>
        [HttpGet]
        [ProducesResponseType(typeof(ApiResult<List<Card>>), 200)]
        public ActionResult<ApiResult<List<Card>>> GetCards()
        {
            ApiResult<List<Card>> responseModel = new() { Result = CardCollection.Cards };

            return Ok(responseModel);
        }

        /// <summary>
        /// Get cards by userId
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>Collection of cards that belong to the user</returns>
        /// 
        // todo move to useusers controller
        [HttpGet("users/{userId:guid}/cards")]
        [ProducesResponseType(typeof(ApiResult<List<Card>>), 200)]

        // todo 404 or  204
        [ProducesResponseType(typeof(ApiResult<List<Card>>), 204)]
        [ProducesResponseType(typeof(ApiResult<List<Card>>), 400)]
        [ProducesResponseType(typeof(ApiResult<List<Card>>), 404)]
        public ActionResult<ApiResult<List<Card>>> GetCardsByUserId(Guid? userId)
        {
            var responseModel = new ApiResult<List<Card>>();

            if (!userId.HasValue)
            {
                responseModel.ErrorCode = ErrorCodes.UserNotFound;
                responseModel.ErrorMessage = "UserId shouldn't be null";
                return BadRequest(responseModel);
            }

            // todo move to repositories
            responseModel.Result = CardCollection.Cards
                .Where(c => c.UserId == userId)
                .ToList();

            return Ok(responseModel);
        }

        /// <summary>
        /// Get card by Id
        /// </summary>
        /// <param name="cardId"></param>
        /// <returns>Card</returns>
        [HttpGet("{cardId:guid}")]
        [ProducesResponseType(typeof(ApiResponseModel<Card>), 200)]
        [ProducesResponseType(typeof(ApiResponseModel<Card>), 400)]
        [ProducesResponseType(typeof(ApiResponseModel<Card>), 404)]
        public ActionResult<ApiResponseModel<Card>> GetCardById(Guid cardId)
        {
            ApiResponseModel<Card> responseModel =
                new ApiResponseModel<Card>();

            if (cardId != default)
            {
                responseModel.ErrorCode = ErrorCodes.CardIdIsEmpty;
                responseModel.ErrorMessage = "CardId shouldn't be null";
                return BadRequest(responseModel);
            }

            responseModel.Result = CardCollection.Cards
                .Where(c => c.Id == cardId)
                .FirstOrDefault();

            if (responseModel.Result == null)
            {
                responseModel.ErrorCode = ErrorCodes.CardNotFound;
                responseModel.ErrorMessage = "Card is't found";
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
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponseModel<Card>), 200)]
        [ProducesResponseType(typeof(ApiResponseModel<Card>), 400)]
        [ProducesResponseType(typeof(ApiResponseModel<Card>), 409)]
        public ActionResult<ApiResponseModel<Card>> CreateCard([FromBody] CreateCardRequest createCardRequest)
        {
            // validation of model 
            // tryValidate
            // return 400 errorCode = BadRequest

            // getCardByPanOrId
            // if model is not empty from repo, return errorCode = CardAlreadyCreated
            if (CardCollection.Cards.Any(c => c.Id == card.Id))
            {
                responseModel.ErrorCode = ErrorCodes.InvalidModel;
                responseModel.ErrorMessage = "Id is already in use";
                return Conflict(responseModel);
            }

            if (CardCollection.Cards.Any(c => c.Pan == card.Pan))
            {
                responseModel.ErrorCode = ErrorCodes.InvalidModel;
                responseModel.ErrorMessage = "Pan is already in use";
                return Conflict(responseModel);
            }

            // if validation ok, move to repo
            // todo move to repo
            // guid id = CreateCardAsync (createCardRequest)
                        CardCollection.Cards.Add(card);

            var result = new ApiResult<int>
            {
                Result = cardId
            };

            return //Created(result);
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
        [HttpPut("{id:guid}/changeName")]
        [ProducesResponseType(typeof(Card), 200)]
        [ProducesResponseType(typeof(BadRequestObjectResult), 400)]
        [ProducesResponseType(typeof(ApiError), 404)]
        public IActionResult EditCardName([FromQuery] Guid id, [FromBody] string name)
        {
            // check if name is empty or null, return 400

            // get card by id, if not found return 404

            // return ok

            var card = CardCollection.Cards.FirstOrDefault(c => c.Id == id);

            if (card == null)
                return NotFound(new ApiError()
                {
                    Result = BusinessResult.NotFound,
                    Message = "Card not found"
                });

            card.Name = name;

            return Ok();
        }

        /// <summary>
        /// Delete a card by id
        /// </summary>
        /// <param name="id"></param>
        /// <response code="200">OK</response>
        /// <response code="404">Card not found</response>
        /// <returns>OK if deletion is successful</returns>
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(200)]
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

    public class CreateCardRequest
    {
        [StringLength(3)]
        public string Cvc { get; set; }

        /// <summary>
        /// Card Number
        /// </summary>
        [PanValidation(ErrorMessage = "Card Number is invalid")]
        public string Pan { get; set; }

        /// <summary>
        /// Month and Year
        /// </summary>
        [Required]
        [ExpireValidation(ErrorMessage = "Month or year is less than current month or year")]
        public Expire Expire { get; set; }

        public string Name { get; set; }

        public bool IsDefault { get; set; }

        public Guid UserId { get; set; }

    }
}
