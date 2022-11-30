using DeliveryBackend.Data;
using DeliveryBackend.Data.Models;
using DeliveryBackend.Data.Models.DTO;

namespace DeliveryBackend.Services;

public class WeatherService : IWeatherService
{
    private readonly ApplicationDbContext _context;

    public WeatherService(ApplicationDbContext context)
    {
        _context = context;
    }

    public WeatherForecastDto[] GenerateWeather()
    {
        return _context.WeatherForecasts.Select(x => new WeatherForecastDto
        {
            Date = x.Date,
            Summary = x.Summary,
            TemperatureC = x.TemperatureC
        }).ToArray();
    }

    public async Task Add(WeatherForecastDto model)
    {
        await _context.WeatherForecasts.AddAsync(new WeatherForecast
        {
            Date = model.Date,
            Summary = model.Summary,
            TemperatureC = model.TemperatureC
        });
        await _context.SaveChangesAsync();
    }
}