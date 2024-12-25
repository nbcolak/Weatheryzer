using MediatR;
using Weatheryzer.Application.Dtos;
using Weatheryzer.Shared.Responses;

namespace Weatheryzer.Application.WeatherForecast.Queries;

public record GetWeatherByCityQuery(string City) : IRequest<Response<List<WeatherForecastDto>>>;
