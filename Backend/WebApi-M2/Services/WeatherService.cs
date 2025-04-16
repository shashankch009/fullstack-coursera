using WebApi_M2.Models;

namespace WebApi_M2.Services;

public class WeatherService 
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    public IEnumerable<WeatherForecast> GetAll()
    {
        var rng = new Random();
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast(
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            rng.Next(-20, 55),
            Summaries[rng.Next(Summaries.Length)])).ToArray();
    }
}