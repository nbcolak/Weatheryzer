using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Weatheryzer.Domain.Entities;

[Table("weatherdata", Schema = "public")]
public class WeatherData
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("city")]
    [MaxLength(100)]
    public string City { get; set; }

    [Column("data")] // 'data' sütunu JSON formatında olabilir.
    public string Data { get; set; }

    [Column("temperature_c")]
    public double TemperatureC { get; set; } // Sıcaklık bilgisi

    [Column("weather_condition")]
    [MaxLength(50)]
    public string WeatherCondition { get; set; } // Hava durumu bilgisi

    [Column("timestamp")]
    public DateTime Timestamp { get; set; }
}