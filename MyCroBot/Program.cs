using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MyCroBot;

var serviceCollection = new ServiceCollection()
    .AddLogging(options =>
    {
        options.ClearProviders();
        options.AddConsole();
    });

serviceCollection.AddSingleton<IConfiguration>(p =>
{
    var builder = new ConfigurationBuilder();
    builder.SetBasePath(Directory.GetCurrentDirectory())
       .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
       .AddJsonFile("App_Data/appsettings.json", optional: true, reloadOnChange: true);

    return builder.Build();
});

var startup = new Startup();
startup.ConfigureServices(serviceCollection);

var serviceProvider = serviceCollection.BuildServiceProvider();

startup.Configure(serviceProvider);

startup.Run(null, serviceProvider);