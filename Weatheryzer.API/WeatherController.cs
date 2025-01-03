using MediatR;
using Microsoft.AspNetCore.Mvc;
using Weatheryzer.Application.WeatherForecast.Command;
using Weatheryzer.Application.WeatherForecast.Queries;

namespace Weatheryzer.API;

[ApiController]
[Route("api/[controller]")]
public class WeatherController(IMediator mediator, ILogger<WeatherController> logger) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateWeather([FromBody] CreateWeatherCommand command)
    {
        logger.LogInformation("CreateWeather endpoint called with payload: {@Command}", command);

        try
        {
            var response = await mediator.Send(command);
            if (!response.Success)
            {
                logger.LogWarning("CreateWeather failed with status code: {StatusCode} and message: {Message}",
                    response.StatusCode.Code, response.Message);
                return StatusCode(response.StatusCode.Code, response.Message);
            }

            logger.LogInformation("CreateWeather succeeded for city: {City} with response: {@Response}",
                command.City, response);

            return CreatedAtAction(nameof(GetWeatherByCity), new { city = command.City }, response);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while processing CreateWeather for payload: {@Command}", command);
            return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
        }
    }

    [HttpGet("{city}")]
    public async Task<IActionResult> GetWeatherByCity(string city)
    {
        logger.LogInformation("Weather data requested.");
        try
        {
            var response = await mediator.Send(new GetWeatherByCityQuery(city));

            if (!response.Success)
                return StatusCode(response.StatusCode.Code, response.Message);
            return Ok(response);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while retrieving weather data.");
            return StatusCode(500, "Internal server error.");
        }
    }
}