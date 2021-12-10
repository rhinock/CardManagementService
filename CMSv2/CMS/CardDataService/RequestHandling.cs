using System;
using System.IO;
using System.Text;
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

        public override async Task OnGet(HttpContext context)
        {
            string path = context.Request.Path.Value.ToLower();

            if (path.StartsWith($"/{Options.Get<string>("Prefix")}("))
            {
                Guid id = GetItemId(context);

                Card card = Repository.Get<Card>(x => x.Id == id);
            }
            else
            {
                Guid emptyId = Guid.Empty;
                IEnumerable<Card> cards = Repository.GetMany<Card>(x => x.UserId != emptyId);
                await SetResponseObject(context, new { value = cards });
            }
        }

        public override async Task OnPatch(HttpContext context)
        {
            Guid id = GetItemId(context);

            Card card = Repository.Get<Card>(x => x.Id == id);
            Card newData = JsonConvert.DeserializeObject<Card>(GetBodyContent(context));

            card.Set(newData);
            await Repository.Update(card);

            context.Response.StatusCode = 204;
        }

        public override async Task OnPost(HttpContext context)
        {
            Card newData = JsonConvert.DeserializeObject<Card>(GetBodyContent(context));
            await Repository.Create(newData);

            context.Response.StatusCode = 201;
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

        private string GetBodyContent(HttpContext context)
        {
            string body;

            using (StreamReader reader = new StreamReader(context.Request.Body, Encoding.UTF8))
            {
                body = reader.ReadToEnd();
            }

            return body;
        }

        private async Task SetResponseObject<T>(HttpContext context, T data)
        {
            context.Response.StatusCode = 200;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(JsonConvert.SerializeObject(data));
        }
    }
}
