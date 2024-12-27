using Azure.Storage.Blobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Weatheryzer.Function.Options;
using Weatheryzer.Function.Services;
using Weatheryzer.Function.Services.Interfaces;

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
        // HttpClient 
        services.AddHttpClient<IWeatherService, WeatherService>();

        // Blob Service Client
        services.AddSingleton(x => 
            new BlobServiceClient(Environment.GetEnvironmentVariable("MyAzureStorageBlob"))
        );
        services.AddSingleton<IRetryPolicyService,RetryPolicyService>();
    })
    .Build();

host.Run();