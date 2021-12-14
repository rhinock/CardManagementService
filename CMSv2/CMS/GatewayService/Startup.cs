using Domain.Objects;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System.Collections.Generic;
using WebTools;

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

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            _resourceConnections = Configuration.GetSection("ConnectionResources").Get<Dictionary<string, ResourceConnection>>();
            services.Add(new ServiceDescriptor(typeof(Dictionary<string, ResourceConnection>), _resourceConnections));

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "GatewayService", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
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

            MiddlewareOptions options = new MiddlewareOptions();
            options.Add("Auth", _resourceConnections["Auth"]);
            app.UseMiddleware<Authentication>(options);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
