using WebTools;

using Domain.Objects;

using System.Collections.Generic;

using GatewayService.Attributes;

using Microsoft.OpenApi.Models;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GatewayService
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        private Dictionary<string, ResourceConnection> _resourceConnections;

        public void ConfigureServices(IServiceCollection services)
        {
            _resourceConnections = Configuration.GetSection("ConnectionResources").Get<Dictionary<string, ResourceConnection>>();
            services.Add(new ServiceDescriptor(typeof(Dictionary<string, ResourceConnection>), _resourceConnections));
            services.AddScoped<LoggingAttribute>();

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "GatewayService", Version = "v1" });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "GatewayService v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseMiddleware<ErrorHandling>(new MiddlewareOptions(new Dictionary<string, object>
            {
                { "Logger", _resourceConnections["Logger"] }
            }));

            app.UseMiddleware<Authentication>(new MiddlewareOptions(new Dictionary<string, object> 
            {
                { "Auth", _resourceConnections["Auth"] }
            }));

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
