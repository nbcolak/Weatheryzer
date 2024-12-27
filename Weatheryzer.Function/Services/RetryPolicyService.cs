using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Polly;
using Weatheryzer.Function.Options;

namespace Weatheryzer.Function.Services;

public class RetryPolicyService(ILogger<RetryPolicyService> logger, IOptions<RetryPolicyOptions> options)
{
    private readonly RetryPolicyOptions _options = options.Value;

    public IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
    {
        return Policy
            .Handle<HttpRequestException>()
            .OrResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode)
            .WaitAndRetryAsync(
                _options.MaxRetries,
                retryAttempt => TimeSpan.FromSeconds(_options.InitialDelaySeconds * retryAttempt),
                (outcome, timespan, retryAttempt, context) =>
                {
                    logger.LogWarning($"Retrying API call. Attempt: {retryAttempt}, Waiting: {timespan.TotalSeconds} seconds.");
                });
    }
}