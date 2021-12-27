using WebTools;

using Infrastructure;

using Domain.Objects;
using Domain.Interfaces;

using Data.RightsService;
using Data.RightsService.Objects;

using System.Collections.Generic;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace RightsService
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

            var users = Configuration
                .GetSection("Users")
                .Get<User[]>();

            ResourceConnection mainResourceConnection = resourceConnections["MainData"];

            IDataSchema dataSchema = DataSchemaManager.GetDataSchema<MigrationContext>(mainResourceConnection);
            dataSchema.Actualize(users);
            dataSchema.Dispose();

            app.UseMiddleware<ErrorHandling>(new MiddlewareOptions(new Dictionary<string, object>
            {
                { "Logger", resourceConnections["Logger"] },
            }));

            app.UseMiddleware<RequestHandling>(new MiddlewareOptions(new Dictionary<string, object>
            {
                { "Prefix", "operation" },
                { "MainData", mainResourceConnection }
            }));
        }
    }
}