using DeliveryBackend.Data.Models.DTO;

namespace DeliveryBackend.Services;
public interface IWeatherService
{
    WeatherForecastDto[] GenerateWeather();
    Task Add(WeatherForecastDto model);
}
