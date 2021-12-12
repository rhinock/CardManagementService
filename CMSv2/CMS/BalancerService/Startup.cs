using WebTools;

using Domain.Objects;

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
            var resourceConnections = _config.GetSection("ConnectionResources").Get<Dictionary<string, ResourceConnection>>();

            MiddlewareOptions options = new MiddlewareOptions();
            options.Add("MainData", resourceConnections["MainData"]);

            app.UseMiddleware<RequestHandling>(options);
        }
    }
}