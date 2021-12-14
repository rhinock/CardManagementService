using Domain.Interfaces;
using Domain.Objects;
using GatewayService.Enums;
using GatewayService.ResponseModels;
using Infrastructure;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using WebTools;
using WebTools.Middlewares;

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
