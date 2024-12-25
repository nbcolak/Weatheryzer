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
        var command = new CreateWeatherCommand(
            City: "Istanbulxxx", 
            TemperatureC: 25.5, 
            Condition: "Sunny", 
            Humidity: 75, 
            WindSpeed: 15, 
            AdditionalInfo: "Test Additional Info");
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