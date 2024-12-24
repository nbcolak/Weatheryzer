using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Weatheryzer.Application.Interfaces;
using Weatheryzer.Application.WeatherForecast.Profiles;
using Weatheryzer.Application.WeatherForecast.Services;
using Weatheryzer.Application.WeatherForecast.Validators;

namespace Weatheryzer.Application;

public static class ApplicationServiceRegistration
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IWeatherForecastService, WeatherForecastService>();
        services.AddAutoMapper(typeof(WeatherProfile));
        services.AddValidatorsFromAssemblyContaining<WeatherRequestDtoValidator>();

        return services;
    }
}