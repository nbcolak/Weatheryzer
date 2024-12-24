namespace Weatheryzer.Application.Dtos;

public record WeatherForecastDto(
    DateTime Date,
    string WeatherCondition,
    double TemperatureC,
    string City)
{
    public double TemperatureF => 32 + (TemperatureC / 0.5556);
}