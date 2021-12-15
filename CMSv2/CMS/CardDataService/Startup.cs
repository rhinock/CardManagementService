using CardDataService.Objects;
using Domain.Interfaces;
using Domain.Objects;
using Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PgDataStore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebTools;

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
            var resourceConnections = _config.GetSection("ConnectionResources").Get<Dictionary<string, ResourceConnection>>();

            var requestHandlingOptions = new MiddlewareOptions();
            requestHandlingOptions.Add("Prefix", "card");
            requestHandlingOptions.Add("MainData", resourceConnections["MainData"]);

            IEvents events = resourceConnections["MessageData"].Events();
            events.Handle(evnt =>
            {
                MessageHandling handling = new MessageHandling(resourceConnections["MainData"]);
                handling.Run(evnt);
            });

            app.UseMiddleware<RequestHandling>(requestHandlingOptions);
        }
    }
}