namespace Weatheryzer.Application.Dtos;

public record WeatherRequestDto(
    string City,
    DateTime Date);