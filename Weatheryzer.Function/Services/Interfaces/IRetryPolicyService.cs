using Polly;

namespace Weatheryzer.Function.Services.Interfaces;

public interface IRetryPolicyService
{
    Task<T> ExecuteAsync<T>(Func<Task<T>> operation);
    IAsyncPolicy<HttpResponseMessage> GetRetryPolicy();
}