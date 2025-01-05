using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exchange.Api;
using Exchange.Api.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyCroBot.Services;
using MyCroBot.Strategies;

namespace MyCroBot
{
    internal class Startup
    {
        internal void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IRequestBuilder, RequestBuilder>(serviceProvider =>
                {
                    var configuration = serviceProvider.GetRequiredService<IConfiguration>();
                    var apiKey = configuration["Exchange:ApiKey"];
                    var apiSecret = configuration["Exchange:ApiSecret"];
                    return new RequestBuilder(apiKey, apiSecret);
                });
            
            services.AddSingleton<IUserService, UserService>();
            services.AddSingleton<IMarketService, MarketService>();
            services.AddSingleton<ITradingService, TradingService>();
            services.AddSingleton<IHistoryService, HistoryService>();
            services.AddSingleton<BasicStrategy>();
            services.AddSingleton<BotRunner>();
        }

        internal void Configure(IServiceProvider serviceProvider)
        {

        }

        internal int Run(string[] args, ServiceProvider services)
        {
            Console.WriteLine("RUNNING...");
            var botRunner = services.GetRequiredService<BotRunner>();
            botRunner.Run();

            return 0;
        }
    }
}
