using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Weatheryzer.Function.Options;
using Weatheryzer.Function.Services;
using Weatheryzer.Function.Services.Interfaces;

namespace Weatheryzer.Tests.Functions;

public class RetryPolicyServiceTests
{
    private readonly Mock<ILogger<RetryPolicyService>> _mockLogger;
    private readonly IRetryPolicyService _retryPolicyService;

    public RetryPolicyServiceTests()
    {
        // Mock the ILogger
        _mockLogger = new Mock<ILogger<RetryPolicyService>>();

        // Mock the IOptions<RetryPolicyOptions>
        var mockOptions = Options.Create(new RetryPolicyOptions
        {
            MaxRetries = 3,
            InitialDelaySeconds = 2
        });

        // Instantiate RetryPolicyService with mocks
        _retryPolicyService = new RetryPolicyService(_mockLogger.Object, mockOptions);
    }

    [Fact]
    public async Task ExecuteAsync_Should_Retry_On_Exception()
    {
        // Arrange
        int executionCount = 0;
        Func<Task<string>> failingOperation = async () =>
        {
            executionCount++;
            throw new Exception("Test Exception");
        };

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => _retryPolicyService.ExecuteAsync(failingOperation));

        // Assert
        executionCount.Should().Be(4); // 1 initial + 3 retries
    }

    [Fact]
    public async Task ExecuteAsync_Should_Succeed_When_No_Exception()
    {
        // Arrange
        Func<Task<string>> successOperation = async () => "Success";

        // Act
        var result = await _retryPolicyService.ExecuteAsync(successOperation);

        // Assert
        result.Should().Be("Success");
    }

    [Fact]
    public async Task ExecuteAsync_Should_Wait_Between_Retries()
    {
        // Arrange
        var startTime = DateTime.UtcNow;
        Func<Task<string>> failingOperation = async () =>
        {
            throw new Exception("Test Exception");
        };

        // Act
        await Assert.ThrowsAsync<Exception>(() => _retryPolicyService.ExecuteAsync(failingOperation));

        var endTime = DateTime.UtcNow;

        // Assert
        (endTime - startTime).Should().BeGreaterThan(TimeSpan.FromSeconds(6)); // 3 retries x 2 seconds each
    }
}