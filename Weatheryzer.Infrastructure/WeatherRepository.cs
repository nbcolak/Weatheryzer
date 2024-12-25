
using Weatheryzer.Domain.Entities;

namespace Weatheryzer.Infrastructure;


public class WeatherRepository : Repository<WeatherData>
{
    public WeatherRepository(WeatherDbContext context) : base(context)
    {
    }

}