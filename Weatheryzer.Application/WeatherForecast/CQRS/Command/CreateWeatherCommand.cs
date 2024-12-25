using Weatheryzer.Shared.Responses;

namespace Weatheryzer.Application.WeatherForecast.Command;

using MediatR;

public record CreateWeatherCommand(
    string City,
    double TemperatureC,
    string Condition,
    int Humidity,
    double WindSpeed,
    string AdditionalInfo
) : IRequest<Response<int>>;