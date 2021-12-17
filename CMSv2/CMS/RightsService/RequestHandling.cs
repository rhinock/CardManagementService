using WebTools;
using WebTools.Middlewares;

using Domain.Objects;
using Domain.Interfaces;

using Infrastructure;

using Newtonsoft.Json;

using RightsService.Objects;

using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;

namespace RightsService
{
    public class RequestHandling : BaseMiddleware
    {
        private readonly IRepository _repository;

        public RequestHandling(RequestDelegate next, MiddlewareOptions options) : base(next, options)
        {
            _repository = RepositoryManager.GetRepository(options.Get<ResourceConnection>("MainData"));
        }

        public override async Task InvokeAsync(HttpContext context)
        {
            string path = context.Request.Path.Value.ToLower();

            if (path == "/login")
            {
                if (context.Request.Method != "POST")
                {
                    context.Response.StatusCode = 405;
                    return;
                }

                string data = await context.Request.GetBodyAsStringAsync();
                User userData = JsonConvert.DeserializeObject<User>(data);
                string userName = userData?.Name;

                User user = await _repository.Get<User>(x => x.Name == userName);
                if (user == null || user.Password != null && user.Password != userData.Password)
                {
                    context.Response.StatusCode = 401;
                    return;
                }

                context.Response.StatusCode = 200;
                await context.Response.WriteAsJsonAsync(new { AccessToken = user.AccessToken });
            }
            else if (path == "/cred")
            {
                string value = context.Request.Query["value"];
                if(string.IsNullOrEmpty(value))
                {
                    context.Response.StatusCode = 400;
                    return;
                }

                User user = await _repository.Get<User>(x => x.AccessToken == value);
                if (user == null)
                {
                    context.Response.StatusCode = 400;
                    return;
                }

                context.Response.StatusCode = 200;
                await context.Response.WriteAsJsonAsync(new { Identity = user.Name });
            }
            else
            {
                context.Response.StatusCode = 404;
            }
        }
    }
}
