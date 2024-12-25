using Microsoft.Extensions.Configuration;
using Weatheryzer.Shared.Interfaces;

namespace Weatheryzer.Infrastructure;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

public static class InfrastructureServiceRegistration
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<WeatherDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}