namespace Weatheryzer.Infrastructure.WeatherForecast.Interfaces;

public interface IWeatherApiClient
{
    Task<WeatherApiResponse> GetForecastAsync(string city);
}