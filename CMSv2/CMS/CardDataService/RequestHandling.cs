﻿using System;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;

using Domain.Objects;
using Domain.Interfaces;

using WebTools;
using WebTools.Middlewares;

using ObjectTools;

using Newtonsoft.Json;

using CardDataService.Objects;

using Microsoft.AspNetCore.Http;
using Infrastructure;

namespace CardDataService
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

                Card card = await Repository.Get<Card>(x => x.Id == id);
                await SetResponseObject(context, card);
            }
            else
            {
                IEnumerable<Card> cards;
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
                    cards = await Repository.GetMany(term.ToBoolExpression<Card>());
                }
                else
                {
                    cards = await Repository.GetMany<Card>();
                }
                await SetResponseObject(context, new { value = cards });
            }
        }

        protected override async Task OnPatch(HttpContext context)
        {
            Guid id = GetItemId(context);

            Card card = await Repository.Get<Card>(x => x.Id == id);
            Card newData = JsonConvert.DeserializeObject<Card>(await GetBodyContent(context));

            card.Set(newData);
            card.Id = id;
            await Repository.Update(card);

            context.Response.StatusCode = 204;
        }

        protected override async Task OnPost(HttpContext context)
        {
            Card newData = JsonConvert.DeserializeObject<Card>(await GetBodyContent(context));
            await Repository.Create(newData);

            context.Response.StatusCode = 201;
        }

        protected override async Task OnDelete(HttpContext context)
        {
            Guid id = GetItemId(context);

            Card card = await Repository.Get<Card>(x => x.Id == id);
            await Repository.Delete(card);

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
                    innerException = ex?.InnerException?.ToString()
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