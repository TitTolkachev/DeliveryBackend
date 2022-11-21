using DeliveryBackend.Data.Models.DTO;
using DeliveryBackend.Services;
using Microsoft.AspNetCore.Mvc;

namespace DeliveryBackend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WeatherForecastController : ControllerBase
{
    private readonly ILogger<WeatherForecastController> _logger;
    private readonly IWeatherService _weatherService;

    public WeatherForecastController(ILogger<WeatherForecastController> logger,
        IWeatherService weatherService)
    {
        _logger = logger;
        _weatherService = weatherService;
    }

    [HttpGet]
    public IEnumerable<WeatherForecastDto> Get()
    {
        return _weatherService.GenerateWeather();
    }

    [HttpPost]
    public async Task<IActionResult> Post(WeatherForecastDto model)
    {
        if (!ModelState.IsValid)
        {
            return StatusCode(401, "Model is incorrect");
        }

        try
        {
            await _weatherService.Add(model);
            return Ok();
        }
        catch
        {
            return StatusCode(500, "Errors during adding new weather forecast");
        }
    }
}