using Microsoft.AspNetCore.Mvc;

namespace WebApi_M2.Controllers;

[ApiController]
[Route("[controller]")]
public class FileController : ControllerBase
{
    [HttpGet("{*filepath}")]
    public IActionResult Get(string filepath)
    {
        // Example: Return the file path
        return Ok($"Requested file path: {filepath}");
    }

    [HttpGet]
    public IActionResult Get(int? id) 
    {
        // Example: Return the filename
        return Ok($"Requested file ID: {id.GetValueOrDefault()}");
    }

}