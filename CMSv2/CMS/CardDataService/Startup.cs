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
            mainResourceConnection.DataTool<Card>().TryInitData();
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