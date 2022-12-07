// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
//
// Generated with Bot Builder V4 SDK Template for Visual Studio EchoBot v4.18.1

using Common.Middlewares;
using Common.Middlewears;
using MeetingBot.Bots;
using MeetingBot.Controllers;
using MeetingBot.Extensions;
using MeetingBot.Helpers;
using MeetingBot.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Integration.AspNet.Core;
using Microsoft.Bot.Connector.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace MeetingBot
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public IConfiguration configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpClient().AddControllers().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.MaxDepth = HttpHelper.BotMessageSerializerSettings.MaxDepth;
            });

            // Create the Bot Framework Authentication to be used with the Bot Adapter.
            services.AddSingleton<BotFrameworkAuthentication, ConfigurationBotFrameworkAuthentication>();

            services.AddSwaggerGen();

            // Create the Bot Adapter with error handling enabled.
            services.AddSingleton<IBotFrameworkHttpAdapter, AdapterWithErrorHandler>();

            services.AddSingleton<IGraph, GraphHelper>();
            services.ConfigureGraphComponent(options => this.configuration.Bind("AzureAd", options));
 
            services.AddScoped<IMeeting, MeetingHelper>();
            services.AddScoped<IUserProvider, UserProvider>();

            // Create the bot as a transient. In this case the ASP Controller is expecting an IBot.

            services.AddTransient<IBot, CallingBot>();
            services.AddBot(options => this.configuration.Bind("Bot", options));
            //ervices.AddBot<IBot>(options => this.configuration.Bind("Bot", options));
            //services.AddBot<IBot>();
            services.AddCors(options => options.AddDefaultPolicy(builder =>
            {
                builder
                .WithOrigins(configuration[Common.Constants.BotBaseUrlConfigurationSettingsKey])
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
            }));

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseDefaultFiles()
                .UseStaticFiles()
                .UseWebSockets()
                .UseRouting()
                .UseAuthorization()
                .UseSwagger()
                .UseSwaggerUI(s =>
                {
                    s.SwaggerEndpoint("/swagger/v1/swagger.json", "meetbot");
                    //s.RoutePrefix = string.Empty;
                })
                .UseMiddleware<TraceLoggerMiddleware>()
                .UseMiddleware<ErrorHandlerMiddleware>()
                .UseEndpoints(endpoints =>
                {
                    endpoints.MapControllers();
                });

            // app.UseHttpsRedirection();
        }
    }
}
