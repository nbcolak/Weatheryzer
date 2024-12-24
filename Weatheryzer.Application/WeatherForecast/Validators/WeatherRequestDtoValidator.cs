using Weatheryzer.Application.Dtos;
using FluentValidation;

namespace Weatheryzer.Application.WeatherForecast.Validators;

public class WeatherRequestDtoValidator : AbstractValidator<WeatherRequestDto>
{
    public WeatherRequestDtoValidator()
    {
        RuleFor(x => x.City).NotEmpty().WithMessage("City is required.");
        RuleFor(x => x.Date).GreaterThan(DateTime.UtcNow).WithMessage("Date must be in the future.");
    }
}