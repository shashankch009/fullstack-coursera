
using DB1.Models;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class EmployeesController : ControllerBase 
{
    private readonly HRDbService _hrDbService;
    
    public EmployeesController(HRDbService hrDbService)
    {
        _hrDbService = hrDbService;
    }

    [HttpGet]
    public async Task<IActionResult> GetEmployees()
    {
        var employees = await _hrDbService.GetEmployeesAsync();
        return Ok(employees);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetEmployee(int id)
    {
        var employee = await _hrDbService.GetEmployeeByIdAsync(id);
        if (employee == null)
        {
            return NotFound();
        }
        return Ok(employee);
    }

    [HttpPost]
    public async Task<IActionResult> AddEmployee([FromBody] Employee employee)
    {
        if(employee == null)
        {
            return BadRequest("Employee cannot be null");
        }
        await _hrDbService.AddEmployeeAsync(employee);
        return CreatedAtAction(nameof(GetEmployee), new { id = employee.ID }, employee);
    }
}