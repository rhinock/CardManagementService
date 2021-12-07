using CMS.Attributes;
using CMS.Entities;
using CMS.Enums;
using CMS.Extensions;
using CMS.Models;
using CMS.Repositories;
using CMS.ResponseModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.Controllers
{
    /// <summary>
    /// Api controller for operations
    /// </summary>
    [Route("api/operation")]
    public class OperationController : BaseController
    {
        private readonly AppDbContext _appDbContext;
        private readonly IRepository _repository;

        public OperationController(AppDbContext appDbContext, IRepository repository)
        {
            _appDbContext = appDbContext;
            _repository = repository;
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
        public IActionResult GetOperationByCardId(Guid? cardId)
        {
            IEnumerable<Operation> operations;

            if (cardId.HasValue)
            {
                operations = _repository
                    .Query<Operation>()
                    .Where(x => x.CardId == cardId)
                    .ToList();

                return Info(operations.Select(o => o.To<Operation, OperationModel>()));
            }
            else
            {
                Guid defaultCardId = (_repository
                    .Query<Card>()
                    .FirstOrDefault(x => x.IsDefault)?.Id).GetValueOrDefault();

                operations = _repository
                    .Query<Operation>()
                    .Where(x => x.CardId == defaultCardId)
                    .ToList();

                return Info(operations.Select(o => o.To<Operation, OperationModelDefault>()));
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
                card = _repository.Query<Card>().FirstOrDefault(c => c.Id == model.CardId);
            }
            else if (model.Card != null)
            {
                card = model.Card.To<CardShortModel, Card>();
                await _repository.Create(card);
            }
            else
            {
                return await ErrorAsync("CardId should be provided", BusinessResult.InvalidModel);
            }

            if (card ==  null)
            {
                return await ErrorAsync("Card wasn't found", BusinessResult.NotFound);
            }

            operation = model.To<OperationCreateModel, Operation>();
            operation.CardId = card.Id;
            await _repository.Create(operation);

            return await InfoAsync(operation.Id);
        }
    }
}
