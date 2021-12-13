using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using ObjectTools;

using Domain.Objects;

using Microsoft.AspNetCore.Mvc;

using GatewayService.Enums;
using GatewayService.Models;
using GatewayService.Attributes;
using GatewayService.DataObjects;
using GatewayService.ResponseModels;

namespace GatewayService.Controllers
{
    [Route("api/operation")]
    public class OperationController : BaseController
    {
        public OperationController(Dictionary<string, ResourceConnection> connections) : base(connections)
        {
        }

        /// <summary>
        /// Get operation by CardId
        /// </summary>
        /// <param name="cardId"></param>
        /// <returns></returns>
        [HttpGet]
        [Logging]
        [ProducesResponseType(typeof(ResponseModel), 200)]
        [ProducesResponseType(typeof(ResponseModel), 400)]
        public async Task<ActionResult> GetOperationByCardId(Guid? cardId)
        {
            IEnumerable<Operation> operations;

            if (cardId.HasValue)
            {
                Guid operationCardId = cardId.Value;
                operations = await Repository
                    .GetMany<Operation>(x => x.CardId == operationCardId);

                return await InfoAsync(operations.Select(o => o.To<Operation, OperationModel>()));
            }
            else
            {
                Guid defaultCardId = ((await Repository.Get<Card>(x => x.IsDefault == true))?.Id).GetValueOrDefault();

                operations = await Repository
                    .GetMany<Operation>(x => x.CardId == defaultCardId);

                return await InfoAsync(operations.Select(o => o.To<Operation, OperationDefaultModel>()));
            }
        }

        /// <summary>
        /// Create operation
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Operation with new card</returns>
        [Logging]
        [ModelValidation]
        [HttpPost]
        [ProducesResponseType(typeof(ResponseModel), 200)]
        [ProducesResponseType(typeof(ResponseModel), 400)]
        public async Task<ActionResult> CreateOperation([FromBody] OperationCreateModel model)
        {
            Card card;
            Operation operation;

            if (model.CardId.HasValue)
            {
                Guid cardId = model.CardId.Value;
                card = await Repository.Get<Card>(c => c.Id == cardId);
            }
            else if (model.Card != null)
            {
                card = model.Card.To<OperationCardModel, Card>();
                await Repository.Create(card);
            }
            else
            {
                return await ErrorAsync("CardId should be provided", BusinessResult.InvalidModel);
            }

            if (card == null)
            {
                return await ErrorAsync("Card wasn't found", BusinessResult.NotFound);
            }

            operation = model.To<OperationCreateModel, Operation>();
            operation.CardId = card.Id;
            await Repository.Create(operation);

            return await InfoAsync(operation.Id);
        }
    }
}