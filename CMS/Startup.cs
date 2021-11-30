using CMS.Models;
using CMS.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.OpenApi.Models;
using System.Threading.Tasks;
using CMS.Enums;

namespace CMS
{
    public class Startup
    {
        public IConfiguration Configuration { get; set; }

        public Startup(IConfiguration configuration)
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appSettings.Staging.json")
                .AddConfiguration(configuration);

            Configuration = builder.Build();
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            services.Configure<List<Card>>(Configuration);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo() { Title = "CMS" });

                var filePath = Path.Combine(AppContext.BaseDirectory, $"{nameof(CMS)}.xml");
                c.IncludeXmlComments(filePath);

                c.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme()
                {
                    Type = SecuritySchemeType.ApiKey,
                    In = ParameterLocation.Header,
                    Name = "API-KEY",
                    Description = "Api key auth"
                });

                var key = new OpenApiSecurityScheme()
                {
                    Reference = new OpenApiReference()
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "ApiKey"
                    },
                    In = ParameterLocation.Header
                };

                var requirement = new OpenApiSecurityRequirement
                {
                    { key, new List<string>() }
                };

                c.AddSecurityRequirement(requirement);
            });
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
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

            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseReDoc(c => { c.RoutePrefix = "docs"; });

            app.Use(async (context, func) =>
            {
                if (!context.Response.Headers.TryGetValue("API-KEY", out var key) || !key.Equals("secret-api-key"))
                {
                    context.Response.StatusCode = 401;
                    await context.Response.WriteAsJsonAsync(
                        new ApiError()
                        {
                            Result = BusinessResult.Unauthorized,
                            Message = "Invalid api key"
                        });
                    return;
                }

                await func();
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Card}/{action=GetCard}/{id?}");
            });

            app.Run(context =>
            {
                context.Response.Redirect("/docs");
                return Task.FromResult(0);
            });
        }
    }
}
