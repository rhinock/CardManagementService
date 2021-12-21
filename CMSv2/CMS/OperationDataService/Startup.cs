using WebTools;

using Infrastructure;

using Domain.Objects;

using OperationDataService.Objects;

using System.Collections.Generic;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace OperationDataService
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

            var operations = Configuration
                .GetSection("Operations")
                .Get<Operation[]>();

            ResourceConnection mainResourceConnection = resourceConnections["MainData"];
            InitialData initialData = new InitialData(mainResourceConnection, operations);

            if (mainResourceConnection.DataTool<Operation>().TryInitData())
                initialData.Init();

            app.UseMiddleware<ErrorHandling>(new MiddlewareOptions(new Dictionary<string, object>
            {
                { "Logger", resourceConnections["Logger"] },
            }));

            app.UseMiddleware<RequestHandling>(new MiddlewareOptions(new Dictionary<string, object>
            {
                { "Prefix", "operation" },
                { "MainData", mainResourceConnection },
                { "MessageData", resourceConnections["MessageData"] }
            }));
        }
    }
}