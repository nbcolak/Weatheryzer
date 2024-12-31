using Azure.Storage.Blobs;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Weatheryzer.Application;
using Weatheryzer.Function.Options;
using Weatheryzer.Function.Services;
using Weatheryzer.Function.Services.Interfaces;
using Weatheryzer.Infrastructure;

IConfiguration configuration = null;

var host = new HostBuilder()
    .ConfigureFunctionsWebApplication()
    .ConfigureAppConfiguration((context, configBuilder) =>
    {
        configBuilder.AddEnvironmentVariables();
        configBuilder.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
        configuration = configBuilder.Build();

    })
    .ConfigureServices((context, services) =>
    {

        // Options 
        services.Configure<WeatherApiOptions>(configuration.GetSection("WeatherApi"));
        services.Configure<RetryPolicyOptions>(configuration.GetSection("RetryPolicy"));
        
        // Blob Service Client
        services.AddSingleton(x => 
            new BlobServiceClient(Environment.GetEnvironmentVariable("MyAzureStorageBlob"))
        );
        services.AddHttpClient();
        services.AddSingleton<IRetryPolicyService,RetryPolicyService>();
        services.AddSingleton<IBlobStorageService, BlobStorageService>();
        services.AddSingleton<IWeatherService, WeatherService>();
        services.AddScoped<IQueueService, QueueService>();
        services.AddApplicationServices(); 
        services.AddInfrastructureServices(configuration);
        
    })
    .Build();

host.Run();