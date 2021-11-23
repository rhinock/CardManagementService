using CMS.Entities;
using CMS.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;

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
            // services.AddMvc();
            services.AddTransient<IConfiguration>(provider => AppConfiguration);

            var configs = AppConfiguration.GetSection("Cards");
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

            // app.UseRouting();

            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapControllerRoute(
            //        name: "default",
            //        pattern: "{controller=Card}/{action=GetCard}");
            //});

            app.Map("/Config", GetSectionContent);

            app.Map("/Card", card =>
            {
                card.Map("/GetCard", GetCard);
                card.Map("/CreateCard", CreateCard);
            });

            app.Run(async context =>
            {
                context.Response.ContentType = "text/html";
                await context.Response.WriteAsync("Home Page");
            });
        }

        public void GetSectionContent(IApplicationBuilder app)
        {
            app.Run(async context =>
            {
                if (CardCollection.Cards.Count == 0)
                {
                    CardCollection.Cards = AppConfiguration.GetSection("Cards")
                            .GetChildren()
                            .ToList()
                            .Select(c => new Card()
                            {
                                Id = c.GetValue<Guid>("Id"),
                                Cvc = c.GetValue<string>("Cvc"),
                                Pan = c.GetValue<string>("Pan"),
                                Expire = new Expire(c.GetValue<int>("Expire:Month"), c.GetValue<int>("Expire:Year")),
                                Name = $"{AppConfiguration["ASPNETCORE_ENVIRONMENT"]}_{c.GetValue<string>("Name")}",
                                IsDefault = c.GetValue<bool>("IsDefault"),
                                UserId = c.GetValue<Guid>("UserId")
                            })
                            .ToList<Card>();
                }

                var json = string.Empty;

                if (CardCollection.Cards.Count > 0)
                {
                    json = JsonConvert.SerializeObject(CardCollection.Cards);
                    context.Response.StatusCode = StatusCodes.Status200OK;
                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsync(json);
                }
            });
        }

        public void GetCard(IApplicationBuilder app)
        {
            app.Run(async context =>
            {
                try
                {
                    var value = context.Request.PathBase.Value;
                    var json = string.Empty;

                    var userIdFromQuery = context.Request.Query.Where(x => x.Key == "userId").FirstOrDefault();
                    Guid.TryParse(userIdFromQuery.Value, out Guid userId);
                    var cards = CardCollection.GetCardByUserId(userId);

                    if (cards.Any())
                    {
                        json = JsonConvert.SerializeObject(cards);
                    }

                    if (string.IsNullOrEmpty(json))
                    {
                        context.Response.StatusCode = StatusCodes.Status404NotFound;
                        await context.Response.WriteAsync("Not Found");
                    }
                    else
                    {
                        context.Response.StatusCode = StatusCodes.Status200OK;
                        context.Response.ContentType = "application/json";
                        await context.Response.WriteAsync(json);
                    }
                }
                catch (Exception ex)
                {
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    context.Response.ContentType = "text/html";
                    await context.Response.WriteAsync(ex.Message);
                }
            });
        }

        public void CreateCard(IApplicationBuilder app)
        {
            var bodyString = string.Empty;

            app.Run(async context =>
            {
                var bodyStream = context.Request.Body;
                Card card;

                using (var reader = new StreamReader(bodyStream))
                {
                    bodyString = await reader.ReadToEndAsync();
                }

                try
                {
                    card = JsonConvert.DeserializeObject<Card>(bodyString);

                    if (!CardCollection.Cards.Any(c => c.Pan == card.Pan))
                    {
                        card.Id = Guid.NewGuid();
                        CardCollection.AddCard(card);
                    }
                    else
                    {
                        context.Response.StatusCode = StatusCodes.Status400BadRequest;
                        await context.Response.WriteAsync("Pan should be unique");
                        return;
                    }
                }
                catch (Exception ex)
                {
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    await context.Response.WriteAsync(ex.Message);
                }

                context.Response.StatusCode = StatusCodes.Status200OK;
                await context.Response.WriteAsync("Card Added Succesfully");
            });
        }
    }
}
