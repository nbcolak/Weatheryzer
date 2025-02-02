﻿using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Weatheryzer.Application.WeatherForecast.Command;
using Weatheryzer.Domain.Entities;
using Weatheryzer.Infrastructure;
using Weatheryzer.Shared.Interfaces;
using Xunit;

namespace Weatheryzer.Tests;


public class CreateWeatherCommandHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly CreateWeatherCommandHandler _handler;

    public CreateWeatherCommandHandlerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _handler = new CreateWeatherCommandHandler(_unitOfWorkMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnResponse_WhenWeatherDataIsCreated()
    {
        // Arrange
        // Arrange
        var jsonData = JsonSerializer.Serialize(new
        {
            Humidity = 60,
            WindSpeed = 15,
            AdditionalInfo = "Generated by server"
        });

        var command = new CreateWeatherCommand(
            City: "Istanbul",
            TemperatureC: 25.5,
            Condition: "Sunny",
            Data: jsonData // JSON formatında veri
        );

        var weatherData = new WeatherData
        {
            City = command.City,
            TemperatureC = command.TemperatureC,
            WeatherCondition = command.Condition,
            Timestamp = DateTime.UtcNow, // Test için güncel zaman
            Data = command.Data // JSON veriyi buradan alıyoruz
        };
        _unitOfWorkMock.Setup(u => u.Repository<WeatherData>().AddAsync(It.IsAny<WeatherData>()))
            .Returns(Task.CompletedTask);
        _unitOfWorkMock.Setup(u => u.SaveChangesAsync())
            .Returns(Task.CompletedTask);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.Success);
        Assert.Equal(201, result.StatusCode.Code);
        Assert.Equal("Weather data created successfully.", result.Message);
    }
}