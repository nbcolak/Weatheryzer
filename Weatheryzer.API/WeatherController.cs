using MediatR;
using Microsoft.AspNetCore.Mvc;
using Weatheryzer.Application.WeatherForecast.Command;
using Weatheryzer.Application.WeatherForecast.Queries;

namespace Weatheryzer.API;


[ApiController]
[Route("api/[controller]")]
public class WeatherController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateWeather([FromBody] CreateWeatherCommand command)
    {
        var response = await mediator.Send(command);

        if (!response.Success)
            return StatusCode((int)response.StatusCode.Code, response.Message);

        return CreatedAtAction(nameof(GetWeatherByCity), new { city = command.City }, response);
    }

    [HttpGet("{city}")]
    public async Task<IActionResult> GetWeatherByCity(string city)
    {
        var response = await mediator.Send(new GetWeatherByCityQuery(city));

        if (!response.Success)
            return StatusCode((int)response.StatusCode.Code, response.Message);

        return Ok(response);
    }
}