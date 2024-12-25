using System.Text.Json;
using MediatR;
using Weatheryzer.Domain.Entities;
using Weatheryzer.Shared.Enums;
using Weatheryzer.Shared.Interfaces;
using Weatheryzer.Shared.Responses;

namespace Weatheryzer.Application.WeatherForecast.Command;

public class CreateWeatherCommandHandler(IUnitOfWork unitOfWork) : 
        IRequestHandler<CreateWeatherCommand, Response<int>>
{
    public async Task<Response<int>> Handle(CreateWeatherCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var data = new
            {
                Humidity = request.Humidity,
                WindSpeed = request.WindSpeed,
                AdditionalInfo = request.AdditionalInfo
            };
            string jsonData = JsonSerializer.Serialize(data);

            // WeatherData Nesnesi
            var weatherData = new WeatherData
            {
                City = request.City,
                TemperatureC = request.TemperatureC,
                WeatherCondition = request.Condition,
                Data = jsonData, // JSON burada ekleniyor
                Timestamp = DateTime.UtcNow
            };

            // Veritabanına ekleme
            var repository = unitOfWork.Repository<WeatherData>();
            await repository.AddAsync(weatherData);
            await unitOfWork.SaveChangesAsync();


            return new Response<int>(
                weatherData.Id,
                true,
                HttpStatusCodeRecord.Created,
                "Weather data created successfully."
            );
        }
        catch (Exception ex)
        {
            return new Response<int>(
                0,
                false,
                HttpStatusCodeRecord.InternalServerError,
                $"An error occurred: {ex.Message}"
            );
        }
    }
}