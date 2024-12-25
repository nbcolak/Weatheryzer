using Moq;
using Weatheryzer.Application.WeatherForecast.Queries;
using Weatheryzer.Domain.Entities;
using Weatheryzer.Shared.Interfaces;

namespace Weatheryzer.Tests;

public class GetWeatherByCityQueryHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly GetWeatherByCityQueryHandler _handler;

   
    public GetWeatherByCityQueryHandlerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _handler = new GetWeatherByCityQueryHandler(_unitOfWorkMock.Object);
    }
    [Fact]
    public async Task Handle_ShouldReturnWeatherData()
    {
        var query = new GetWeatherByCityQuery("Istanbul");
        var mockData = new List<WeatherData>
        {
            new WeatherData { City = "Istanbul", TemperatureC = 25.5, WeatherCondition = "Sunny" }
        };

        _unitOfWorkMock.Setup(u => u.Repository<WeatherData>().GetAllAsync())
            .ReturnsAsync(mockData);

        var result = await _handler.Handle(query, CancellationToken.None);

        Assert.True(result.Success);
        Assert.Equal(1, result.Data.Count);
        Assert.Equal("Sunny", result.Data[0].WeatherCondition);
    }
}