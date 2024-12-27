using System.Net;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
using Weatheryzer.Function.Options;
using Weatheryzer.Function.Services;
using Weatheryzer.Function.Services.Interfaces;

namespace Weatheryzer.Tests.Functions
{
    public class WeatherServiceTests
    {
        private readonly Mock<ILogger<IWeatherService>> _mockLogger;
        private readonly Mock<IOptions<WeatherApiOptions>> _mockWeatherApiOptions;
        private readonly Mock<IRetryPolicyService> _mockRetryPolicyService;
        private readonly WeatherService _weatherService;
        private readonly HttpClient _httpClient;

        public WeatherServiceTests()
        {
            _mockLogger = new Mock<ILogger<IWeatherService>>();
            _mockWeatherApiOptions = new Mock<IOptions<WeatherApiOptions>>();
            _mockRetryPolicyService = new Mock<IRetryPolicyService>();

            // Mock HttpClient
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("{\"success\": true}")
                });

            _httpClient = new HttpClient(mockHttpMessageHandler.Object)
            {
                BaseAddress = new Uri("https://api.weatherapi.com/v1/")
            };

            // Setup WeatherApiOptions
            _mockWeatherApiOptions.Setup(o => o.Value).Returns(new WeatherApiOptions
            {
                BaseUrl = "https://api.weatherapi.com/v1/",
                ApiKey = "fake-api-key",
                City = "Ankara"
            });

            // Create WeatherService instance
            _weatherService = new WeatherService(
                _httpClient,
                _mockLogger.Object,
                _mockWeatherApiOptions.Object,
                _mockRetryPolicyService.Object);
        }

        [Fact]
        public async Task GetWeatherAsync_Should_Return_SuccessResponse()
        {
            // Arrange
            _mockRetryPolicyService
                .Setup(r => r.ExecuteAsync(It.IsAny<Func<Task<HttpResponseMessage>>>()))
                .Returns((Func<Task<HttpResponseMessage>> operation) => operation());

            // Act
            var response = await _weatherService.GetWeatherAsync();

            // Assert
            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var content = await response.Content.ReadAsStringAsync();
            Assert.Contains("success", content);
        }
    }
}