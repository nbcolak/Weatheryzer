namespace Weatheryzer.Function.Services;

public interface IWeatherService
{
    Task<HttpResponseMessage> GetWeatherAsync();
}