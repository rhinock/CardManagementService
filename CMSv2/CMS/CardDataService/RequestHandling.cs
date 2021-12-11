﻿using System;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;

using Domain.Interfaces;

using WebTools;
using WebTools.Middlewares;

using ObjectTools;

using Newtonsoft.Json;

using CardDataService.Objects;

using Microsoft.AspNetCore.Http;

namespace CardDataService
{
    public class RequestHandling : DataHandlingMiddleware
    {
        public RequestHandling(RequestDelegate requestDelegate, MiddlewareOptions options) : base(requestDelegate, options)
        {
        }

        private IRepository Repository => Options.Get<IRepository>("Repository");

        protected override async Task OnGet(HttpContext context)
        {
            string path = context.Request.Path.Value.ToLower();

            if (path.StartsWith($"/{Options.Get<string>("Prefix")}("))
            {
                Guid id = GetItemId(context);

                Card card = Repository.Get<Card>(x => x.Id == id);
                await SetResponseObject(context, card);
            }
            else
            {
                Guid emptyId = Guid.Empty;
                IEnumerable<Card> cards = Repository.GetMany<Card>(x => x.UserId == emptyId);
                await SetResponseObject(context, new { value = cards });
            }
        }

        protected override async Task OnPatch(HttpContext context)
        {
            Guid id = GetItemId(context);

            Card card = Repository.Get<Card>(x => x.Id == id);
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

            Card card = Repository.Get<Card>(x => x.Id == id);
            await Repository.Delete(card);

            context.Response.StatusCode = 204;
        }

        protected override async Task OnError(HttpContext context, Exception ex)
        {
            context.Response.StatusCode = 500;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(JsonConvert.SerializeObject(
                new { 
                    message = ex.Message, 
                    stackTrace = ex.StackTrace, 
                    innerException = ex.InnerException.ToString()
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