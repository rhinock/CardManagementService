using Domain.Interfaces;
using Domain.Objects;
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

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            var resourceConnections = _config.GetSection("ConnectionResources").Get<Dictionary<string, ResourceConnection>>();
            IRepository repository = new Repository(resourceConnections["MainData"]);

            var requestHandlingOptions = new MiddlewareOptions();
            requestHandlingOptions.Add("Prefix", "card");
            requestHandlingOptions.Add("Repository", repository);

            app.UseMiddleware<RequestHandling>(requestHandlingOptions);
        }
    }
}
