using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Weatheryzer.Application.Interfaces;
using Weatheryzer.Application.WeatherForecast.Command;
using Weatheryzer.Application.WeatherForecast.Profiles;
using Weatheryzer.Application.WeatherForecast.Queries;
using Weatheryzer.Application.WeatherForecast.Services;
using Weatheryzer.Application.WeatherForecast.Validators;
using Weatheryzer.Shared.Interfaces;

namespace Weatheryzer.Application;

public static class ApplicationServiceRegistration
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IWeatherForecastService, WeatherForecastService>();
        services.AddAutoMapper(typeof(WeatherProfile));
        services.AddValidatorsFromAssemblyContaining<WeatherRequestDtoValidator>();
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(ApplicationServiceRegistration).Assembly));
       
        return services;
    }
}