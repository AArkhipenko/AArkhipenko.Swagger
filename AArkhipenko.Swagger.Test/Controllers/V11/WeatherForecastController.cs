using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

using TestConsts = AArkhipenko.Swagger.Test.Consts;

namespace AArkhipenko.Swagger.Test.Controllers.V11;

[ApiController]
[ApiVersion(TestConsts.ApiVersion11, Deprecated = false)]
[Route("weather-forecast/v{version:apiVersion}")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;

    public WeatherForecastController(ILogger<WeatherForecastController> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Получение прогноза
    /// </summary>
    /// <remarks>
    /// Пример запроса:
    ///
    ///     GET /weather-forecast/v11/get
    ///     {
    ///        "id" : 1, 
    ///        "name" : "A4Tech Bloody B188",
    ///        "price" : 111,
    ///        "Type": "PeripheryAndAccessories"
    ///     }
    ///
    /// </remarks>
    /// <returns>Прогноз</returns>
    /// <returns>Список обхектов WeatherForecast</returns>
    /// <response code="200">Запрос выполнен успешно</response>
    [HttpGet("Get")]
    public IEnumerable<WeatherForecast> Get()
    {
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
        .ToArray();
    }
}
