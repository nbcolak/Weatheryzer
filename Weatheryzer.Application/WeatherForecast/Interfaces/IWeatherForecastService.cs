using Weatheryzer.Application.Dtos;

namespace Weatheryzer.Application.Interfaces;

public interface IWeatherForecastService
{
    Task<WeatherForecastDto> GetWeatherForecastAsync(WeatherRequestDto request);
}