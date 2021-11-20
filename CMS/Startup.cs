using CMS.Entities;
using CMS.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;

namespace CMS
{
    public class Startup
    {
        public IConfiguration AppConfiguration { get; set; }

        public Startup(IConfiguration config)
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appSettings.Staging.json")
                .AddConfiguration(config);

            AppConfiguration = builder.Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddTransient<IConfiguration>(provider => AppConfiguration);

            var configs = AppConfiguration.GetSection("Card");
            services.Configure<Card>(configs);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            Action<ILoggingBuilder> loggerConfig = builder => builder.AddConsole();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                loggerConfig = builder => builder.AddDebug();
            }

            var loggerFactory = LoggerFactory.Create(loggerConfig);

            app.UseMiddleware<LoggingMiddleware>();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Card}/{action=GetCard}");
            });
        }
    }
}
