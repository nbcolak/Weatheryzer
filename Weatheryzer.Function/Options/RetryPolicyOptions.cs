namespace Weatheryzer.Function.Options;

public record RetryPolicyOptions
{
    public RetryPolicyOptions() { }

    public RetryPolicyOptions(int maxRetries, int initialDelaySeconds)
    {
        MaxRetries = maxRetries;
        InitialDelaySeconds = initialDelaySeconds;
    }

    public int MaxRetries { get; init; }
    public int InitialDelaySeconds { get; init; }
}