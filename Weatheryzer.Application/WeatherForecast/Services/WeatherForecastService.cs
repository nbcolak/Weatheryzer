using Weatheryzer.Application.Dtos;
using Weatheryzer.Application.Interfaces;

namespace Weatheryzer.Application.WeatherForecast.Services;

public class WeatherForecastService : IWeatherForecastService
{
    public async Task<WeatherForecastDto> GetWeatherForecastAsync(WeatherRequestDto request)
    {
        return await Task.FromResult(CreateMockWeatherForecast(request));
    }

    private WeatherForecastDto CreateMockWeatherForecast(WeatherRequestDto request)
    {
        return new WeatherForecastDto(
            Date: request.Date,
            WeatherCondition: "Sunny",
            TemperatureC: 25.5,
            City: request.City
        );
    }
}