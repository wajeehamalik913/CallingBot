using MeetingBot.Bots;
using MeetingBot.Models.Options;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace MeetingBot.Extensions
{
    public static class BotExtensions
    {
        public static IServiceCollection AddBot(this IServiceCollection services)
    => services.AddBot(_ => { });

        public static IServiceCollection AddBot(this IServiceCollection services, Action<BotOptions> botOptionsAction)
        {
            var options = new BotOptions();
            botOptionsAction(options);
            services.AddSingleton(options);

            return services.AddTransient<CallingBot>();
        }
    }
}
