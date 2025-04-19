
using DB1.Models;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class DepartmentsController : ControllerBase 
{
    private readonly HRDbService _hrDbService;
    
    public DepartmentsController(HRDbService hrDbService)
    {
        _hrDbService = hrDbService;
    }

    [HttpGet]
    public async Task<IActionResult> GetDepartments()
    {
        var departments = await _hrDbService.GetDepartmentsAsync();
        return Ok(departments);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetDepartment(int id)
    {
        var department = await _hrDbService.GetDepartmentByIdAsync(id);
        if (department == null)
        {
            return NotFound();
        }
        return Ok(department);
    }

    [HttpPost]
    public async Task<IActionResult> AddDepartment([FromBody] Department department)
    {
        if(department == null)
        {
            return BadRequest("Department cannot be null");
        }
        await _hrDbService.AddDepartmentAsync(department);
        return CreatedAtAction(nameof(GetDepartment), new { id = department.ID }, department);
    }
}