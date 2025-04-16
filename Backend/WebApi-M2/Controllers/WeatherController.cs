using Microsoft.AspNetCore.Mvc;
using WebApi_M2.Models;
using WebApi_M2.Services;

namespace WebApi_M2.Controllers;

[ApiController]
[Route("[controller]")] // Updated route to explicitly match the expected URL
public class WeatherController : ControllerBase
{
    private WeatherService _weatherService;
    public WeatherController(WeatherService weatherService)
    {
        _weatherService = weatherService;
    }

    [HttpGet]
    public IEnumerable<WeatherForecast> Get()
    {
        return _weatherService.GetAll();
    }

    [HttpPost]
    public IActionResult Post([FromBody] WeatherForecast forecast)
    {
        // Add data to storage (e.g., database)
        return Ok(forecast);
    }

    [HttpPut("{id}")]
    public IActionResult Put(int id, [FromBody] WeatherForecast forecast)
    { 
        // Update data for the given ID
        // Example: Find and update an item with a matching ID
        var existingForecast = new WeatherForecast(forecast.Date, 0, ""); // Replace with actual data retrieval
        //update it based on received info. and save.
        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    { 
        // Delete data for the given ID
        return NoContent();
    }

}