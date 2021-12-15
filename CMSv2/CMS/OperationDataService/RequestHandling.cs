using System;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;

using Domain.Objects;
using Domain.Interfaces;

using WebTools;
using WebTools.Middlewares;

using ObjectTools;

using Newtonsoft.Json;

using OperationDataService.Objects;

using Microsoft.AspNetCore.Http;

using Infrastructure;
using OperationDataService.Models;
using Domain.Enums;

namespace OperationDataService
{
    public class RequestHandling : DataHandlingMiddleware
    {
        public RequestHandling(RequestDelegate requestDelegate, MiddlewareOptions options) : base(requestDelegate, options)
        {
            Repository = Options.Get<ResourceConnection>("MainData").Repository();
        }

        private readonly IRepository Repository;

        protected override async Task OnGet(HttpContext context)
        {
            string path = context.Request.Path.Value.ToLower();

            if (path.StartsWith($"/{Options.Get<string>("Prefix")}("))
            {
                Guid id = GetItemId(context);

                Operation operation = await Repository.Get<Operation>(x => x.Id == id);
                await SetResponseObject(context, operation);
            }
            else
            {
                IEnumerable<Operation> operations;
                if (context.Request.Query.ContainsKey("$filter"))
                {
                    Term term = Term.Create(context.Request.Query["$filter"])
                        .Add("eq", Term.EqualValue)
                        .Add("ne", Term.NotEqualValue)
                        .Add("and", Term.AndValue)
                        .Add("or", Term.OrValue)
                        .Add("gt", Term.GreaterThanValue)
                        .Add("lt", Term.LessThanValue)
                        .Add("ge", Term.GreaterThanOrEqualValue)
                        .Add("le", Term.LessThanValue)
                        .Add("'", Term.QuoteValue);
                    operations = await Repository.GetMany(term.ToBoolExpression<Operation>());
                }
                else
                {
                    operations = await Repository.GetMany<Operation>();
                }
                await SetResponseObject(context, new { value = operations });
            }
        }

        protected override async Task OnPatch(HttpContext context)
        {
            Guid id = GetItemId(context);

            Operation operation = await Repository.Get<Operation>(x => x.Id == id);
            Operation newData = JsonConvert.DeserializeObject<Operation>(await context.Request.GetBodyAsStringAsync());

            operation.Set(newData);
            operation.Id = id;
            await Repository.Update(operation);

            context.Response.Headers.Add("ObjectId", newData.Id.ToString());
            context.Response.StatusCode = 204;
        }

        protected override async Task OnPost(HttpContext context)
        {
            OperationModel model = JsonConvert.DeserializeObject<OperationModel>(await context.Request.GetBodyAsStringAsync());
            Guid id;

            if (model.CardId.HasValue)
            {
                Operation operation = model.To<OperationModel, Operation>();
                await Repository.Create(operation);
                id = operation.Id;
            }
            else
            {
                Operation operation = model.To<OperationModel, Operation>();
                operation.CardId = Guid.NewGuid();
                await Repository.Create(operation);

                model.Card.Id = operation.CardId;

                IEvents events = Options.Get<ResourceConnection>("MessageData").Events();
                events.Add(new Event
                {
                    Arg = model.Card.AsDictionary(),
                    EventType = EventType.MessageAboutCreating
                });

                id = operation.Id;
            }

            context.Response.Headers.Add("ObjectId", id.ToString());
            context.Response.StatusCode = 201;
        }

        protected override async Task OnDelete(HttpContext context)
        {
            Guid id = GetItemId(context);

            Operation operation = await Repository.Get<Operation>(x => x.Id == id);
            await Repository.Delete(operation);

            context.Response.Headers.Add("ObjectId", operation.Id.ToString());
            context.Response.StatusCode = 204;
        }

        protected override async Task OnError(HttpContext context, Exception ex)
        {
            context.Response.StatusCode = 500;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(JsonConvert.SerializeObject(
                new
                {
                    message = ex.Message,
                    stackTrace = ex.StackTrace,
                    innerException = ex.InnerException?.ToString()
                }));
        }

        private Guid GetItemId(HttpContext context)
        {
            string id = context.Request.Path.Value
                .ToLower()
                .Replace($"/{Options.Get<string>("Prefix")}", "")
                .Replace("(", "")
                .Replace(")", "");

            return new Guid(id);
        }
    }
}