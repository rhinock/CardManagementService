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
        [ServiceFilter(typeof(LoggingAttribute))]
        [ProducesResponseType(typeof(ResponseModel), 200)]
        [ProducesResponseType(typeof(ResponseModel), 400)]
        public async Task<ActionResult> GetOperationByCardId(Guid? cardId)
        {
            // IEnumerable<Operation> operations;

            IEnumerable<Operation> operations = await Repository.GetMany<Operation>();
            IEnumerable<OperationModel> operationModels;
            Guid operationCardId;

            if (cardId.HasValue)
            {
                // Guid operationCardId = cardId.Value;
                // operations = await Repository.GetMany<Operation>(x => x.CardId == operationCardId);

                operationCardId = cardId.Value;

                operationModels = operations
                    .Where(x => x.CardId == operationCardId)
                    .ToList()
                    .Select(om => om.To<Operation, OperationModel>());

                // return await InfoAsync(operations.Select(o => o.To<Operation, OperationModel>()));
            }
            else
            {
                // Guid defaultCardId = ((await Repository.Get<Card>(x => x.IsDefault == true))?.Id).GetValueOrDefault();
                // operations = await Repository.GetMany<Operation>(x => x.CardId == defaultCardId);

                // operationCardId = ((await Repository.Get<Card>(x => x.IsDefault == true))?.Id).GetValueOrDefault();
                IEnumerable<Card> cards = await Repository.GetMany<Card>(x => x.IsDefault == true);
                List<Guid> cardIds = cards.Select(x => x.Id).ToList();

                operationModels = operations
                    .Where(x => cardIds.Contains(x.CardId.Value))
                    .ToList()
                    .Select(om => om.To<Operation, OperationModel>());
                
                // return await InfoAsync(operations.Select(o => o.To<Operation, OperationDefaultModel>()));
            }

            return await InfoAsync(operationModels);
        }

        /// <summary>
        /// Create operation
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Operation with new card</returns>
        [ServiceFilter(typeof(LoggingAttribute))]
        [ModelValidation]
        [HttpPost]
        [ProducesResponseType(typeof(ResponseModel), 200)]
        [ProducesResponseType(typeof(ResponseModel), 400)]
        public async Task<ActionResult> CreateOperation([FromBody] OperationCreateModel model)
        {
            Card card;
            Operation operation = model.To<OperationCreateModel, Operation>();

            if (model.CardId.HasValue)
            {
                Guid cardId = model.CardId.Value;
                card = await Repository.Get<Card>(c => c.Id == cardId); 
                
                if (card == null)
                {
                    return await ErrorAsync("Card wasn't found", BusinessResult.NotFound);
                }
                operation.CardId = card.Id;
            }
            else if (model.Card != null)
            {
                card = model.Card.To<OperationCardModel, Card>();
                operation.Card = card;
            }
            else
            {
                return await ErrorAsync("CardId should be provided", BusinessResult.InvalidModel);
            }

            await Repository.Create(operation);

            return await InfoAsync(operation.Id);
        }
    }
}