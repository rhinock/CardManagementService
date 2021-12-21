using WebTools;

using Infrastructure;

using Domain.Objects;
using Domain.Interfaces;

using CardDataService.Objects;

using System.Collections.Generic;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CardDataService
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

            var cards = Configuration
                .GetSection("Cards")
                .Get<Card[]>();

            ResourceConnection mainResourceConnection = resourceConnections["MainData"];
            InitialData initialData = new InitialData(mainResourceConnection, cards);

            if (mainResourceConnection.DataTool<Card>().TryInitData())
                initialData.Init();

            IEvents events = resourceConnections["MessageData"].Events();

            events.Handle(evnt =>
            {
                MessageCatching catching = new MessageCatching(mainResourceConnection);
                catching.Run(evnt);
            });

            app.UseMiddleware<ErrorHandling>(new MiddlewareOptions(new Dictionary<string, object>
            {
                { "Logger", resourceConnections["Logger"] },
            }));

            app.UseMiddleware<RequestHandling>(new MiddlewareOptions(new Dictionary<string, object>
            {
                { "Prefix", "card" },
                { "MainData", mainResourceConnection }
            }));
        }
    }
}