namespace Weatheryzer.Function.Options;

public record WeatherApiOptions
{
    public WeatherApiOptions() { }

    public WeatherApiOptions(string baseUrl, string apiKey, string city)
    {
        BaseUrl = baseUrl;
        ApiKey = apiKey;
        City = city;
    }

    public string BaseUrl { get; init; }
    public string ApiKey { get; init; }
    public string City { get; init; }
}