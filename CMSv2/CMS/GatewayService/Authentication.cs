using WebTools;
using WebTools.Middlewares;

using Infrastructure;

using Domain.Objects;
using Domain.Interfaces;

using GatewayService.Enums;
using GatewayService.ResponseModels;

using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;

namespace GatewayService
{
    public class Authentication : AuthenticationMiddleware
    {
        public Authentication(RequestDelegate next, MiddlewareOptions options = null) : base(next, options)
        {
        }

        private ResourceConnection Connection => Options.Get<ResourceConnection>("Auth");

        public override async Task<bool> Auth(string authorizationValue)
        {
            IUser user = await Connection.User(authorizationValue);

            return user.IsAuth();
        }

        public override void OnError(HttpContext context)
        {
            base.OnError(context);
            context.Response.WriteAsJsonAsync(new ResponseModel
            {
                Result = BusinessResult.Unauthorized
            });
        }
    }
}
