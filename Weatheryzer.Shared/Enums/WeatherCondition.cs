namespace Weatheryzer.Shared.Enums;

public sealed record WeatherCondition(string Name, int Code, string Description)
{
    public static readonly WeatherCondition Sunny = new("Sunny", 1, "Clear sky with lots of sunshine.");
    public static readonly WeatherCondition Cloudy = new("Cloudy", 2, "Overcast or partially cloudy sky.");
    public static readonly WeatherCondition Rainy = new("Rainy", 3, "Rain showers or continuous rain.");
    public static readonly WeatherCondition Snowy = new("Snowy", 4, "Snowfall or snowy conditions.");

    public static IEnumerable<WeatherCondition> List() => new[] { Sunny, Cloudy, Rainy, Snowy };

    public static WeatherCondition FromCode(int code) =>
        List().SingleOrDefault(condition => condition.Code == code) ??
        throw new ArgumentException($"Invalid WeatherCondition code: {code}");

    public static WeatherCondition FromName(string name) =>
        List().SingleOrDefault(condition => condition.Name.Equals(name, StringComparison.OrdinalIgnoreCase)) ??
        throw new ArgumentException($"Invalid WeatherCondition name: {name}");
}