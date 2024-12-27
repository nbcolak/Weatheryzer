
using Google.Protobuf.WellKnownTypes;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Polly;
using Weatheryzer.Function.Options;

namespace Weatheryzer.Function.Services;
public class WeatherService(
    HttpClient httpClient,
    ILogger<IWeatherService> logger,
    IOptions<WeatherApiOptions> weatherApiOptions,
    RetryPolicyService retryPolicyService)
    : IWeatherService
{
    private readonly ILogger<IWeatherService> _logger = logger;

    public async Task<HttpResponseMessage> GetWeatherAsync()
    {
        var policy = retryPolicyService.GetRetryPolicy();

        var url = new UriBuilder(weatherApiOptions.Value.BaseUrl)
        {
            Query = $"key={weatherApiOptions.Value.ApiKey}&q={weatherApiOptions.Value.City}"
        }.Uri;

        var response = await policy.ExecuteAsync(() => 
            httpClient.GetAsync(url));

        return response;
    }
}