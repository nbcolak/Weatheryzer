using System.Text.Json;
using Weatheryzer.Infrastructure.WeatherForecast.Interfaces;

namespace Weatheryzer.Infrastructure.WeatherForecast.Services;

public class WeatherApiClient : IWeatherApiClient
{
    private readonly HttpClient _httpClient;

    public WeatherApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<WeatherApiResponse> GetForecastAsync(string city)
    {
        var response = await _httpClient.GetAsync($"weather?city={city}");
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<WeatherApiResponse>(content);
    }
}