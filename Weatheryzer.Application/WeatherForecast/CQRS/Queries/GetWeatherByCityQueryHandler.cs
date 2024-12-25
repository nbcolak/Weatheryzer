using MediatR;
using Weatheryzer.Application.Dtos;
using Weatheryzer.Domain.Entities;
using Weatheryzer.Shared.Enums;
using Weatheryzer.Shared.Interfaces;
using Weatheryzer.Shared.Responses;

namespace Weatheryzer.Application.WeatherForecast.Queries;

public class GetWeatherByCityQueryHandler : IRequestHandler<GetWeatherByCityQuery, Response<List<WeatherForecastDto>>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetWeatherByCityQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Response<List<WeatherForecastDto>>> Handle(GetWeatherByCityQuery request, CancellationToken cancellationToken)
    {
        try
        {
            // Repository üzerinden veritabanı sorgusu
            var repository = _unitOfWork.Repository<WeatherData>();
            var weatherData = await repository.GetAllAsync();
            var filteredData = weatherData.Where(w => w.City == request.City).ToList();

            if (!filteredData.Any())
            {
                return new Response<List<WeatherForecastDto>>(
                    null,
                    false,
                    HttpStatusCodeRecord.NotFound,
                    "No weather data found for the specified city."
                );
            }

            var weatherForecasts = filteredData.Select(w => new WeatherForecastDto(
                Date: w.Timestamp,
                WeatherCondition: w.WeatherCondition,
                TemperatureC: w.TemperatureC,
                City: w.City)).ToList();

            return new Response<List<WeatherForecastDto>>(
                weatherForecasts,
                true,
                HttpStatusCodeRecord.Ok,
                "Weather data retrieved successfully."
            );
        }
        catch (Exception ex)
        {
            return new Response<List<WeatherForecastDto>>(
                null,
                false,
                HttpStatusCodeRecord.InternalServerError,
                $"An error occurred while retrieving weather data: {ex.Message}"
            );
        }
    }
}