using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using TestConsts = AArkhipenko.Swagger.Test.Consts;

namespace AArkhipenko.Swagger.Test.Controllers.V10;

[ApiController]
[ApiVersion(TestConsts.ApiVersion10, Deprecated = false)]
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

    [HttpGet("get")]
    [SwaggerOperation(Description = "Получение прогноза")]
    [SwaggerResponse(statusCode: StatusCodes.Status201Created, Type = typeof(IEnumerable<WeatherForecast>), Description = "Прогноз")]
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
