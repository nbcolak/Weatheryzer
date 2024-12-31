using Weatheryzer.Shared.Responses;

namespace Weatheryzer.Application.WeatherForecast.Command;

using MediatR;

public record CreateWeatherCommand(
    string City,
    double TemperatureC,
    string Condition,
    string? Data, 
    int? Humidity = null, 
    double? WindSpeed = null, 
    string AdditionalInfo = ""
) : IRequest<Response<int>>
{

}