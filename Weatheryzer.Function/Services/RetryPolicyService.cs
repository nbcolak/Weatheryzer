using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Polly;
using Weatheryzer.Function.Options;
using Weatheryzer.Function.Services.Interfaces;

namespace Weatheryzer.Function.Services;

public class RetryPolicyService(ILogger<RetryPolicyService> logger, IOptions<RetryPolicyOptions> options): IRetryPolicyService
{
    
    public async Task<T> ExecuteAsync<T>(Func<Task<T>> operation)
    {
        var policy = Policy
            .Handle<Exception>()
            .WaitAndRetryAsync(
                options.Value.MaxRetries,
                retryAttempt => TimeSpan.FromSeconds(options.Value.InitialDelaySeconds * retryAttempt),
                (exception, timeSpan, retryCount, context) =>
                {
                    logger.LogWarning($"Retry {retryCount} for operation.");
                });

        return await policy.ExecuteAsync(operation);
    }

    public IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
    {
        return Policy
            .Handle<HttpRequestException>()
            .OrResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode)
            .WaitAndRetryAsync(
                options.Value.MaxRetries,
                retryAttempt => TimeSpan.FromSeconds(options.Value.InitialDelaySeconds * retryAttempt),
                (outcome, timespan, retryAttempt, context) =>
                {
                    logger.LogWarning($"Retrying API call. Attempt: {retryAttempt}, Waiting: {timespan.TotalSeconds} seconds.");
                });
    }
    
}