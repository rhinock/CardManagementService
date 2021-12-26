using WebTools;

using Domain.Objects;

using Infrastructure;

/*using BalancerService.Objects;*/

using System.Collections.Generic;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Data.Balancer.Objects;
using Domain.Interfaces;
using Data.Balancer;

namespace BalancerService
{
    public class Startup
    {
        private readonly IConfiguration Configuration;

        public Startup(IConfiguration configuration)
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile("datasettings.json")
                .AddConfiguration(configuration);

            Configuration = builder.Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            var resourceConnections = Configuration
                .GetSection("ConnectionResources")
                .Get<Dictionary<string, ResourceConnection>>();

            /*var routes = Configuration
                .GetSection("Routes")
                .Get<Route[]>();*/

            ResourceConnection mainResourceConnection = resourceConnections["MainData"];
            /*InitialData initialData = new InitialData(mainResourceConnection, routes);

            if (mainResourceConnection.DataTool<Route>().TryInitData())
                initialData.Init();
            */

            /*IDataTool dataTool = new DataContext<Route>(mainResourceConnection.Value);
            dataTool.TryInitData();*/

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