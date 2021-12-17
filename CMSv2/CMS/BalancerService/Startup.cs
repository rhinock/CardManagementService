using WebTools;

using Domain.Objects;

using Infrastructure;

using BalancerService.Objects;

using System.Collections.Generic;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BalancerService
{
    public class Startup
    {
        private readonly IConfiguration _config;

        public Startup(IConfiguration configuration)
        {
            _config = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            var resourceConnections = _config
                .GetSection("ConnectionResources")
                .Get<Dictionary<string, ResourceConnection>>();

            ResourceConnection mainResourceConnection = resourceConnections["MainData"];

            mainResourceConnection.DataTool<Route>().TryInitData();

            app.UseMiddleware<ErrorHandling>(new MiddlewareOptions(new Dictionary<string, object>
            {
                { "Logger", resourceConnections["Logger"] },
            }));

            app.UseMiddleware<RequestHandling>(new MiddlewareOptions(new Dictionary<string, object>
            {
                { "MainData", mainResourceConnection }
            }));
        }
    }
}