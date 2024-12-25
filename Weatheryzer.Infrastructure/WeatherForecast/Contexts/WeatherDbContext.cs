
using Microsoft.EntityFrameworkCore;
using Weatheryzer.Domain.Entities;

namespace Weatheryzer.Infrastructure
{
    public class WeatherDbContext(DbContextOptions<WeatherDbContext> options) : DbContext(options)
    {
        public DbSet<WeatherData> WeatherData { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<WeatherData>()
                .Property(e => e.Data)
                .HasColumnType("json"); 
        }
    }
    
}