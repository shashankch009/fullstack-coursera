using LogiTrack.Models.Api;
using Microsoft.AspNetCore.Mvc;
using LogiTrack.Services;

namespace LogiTrack.Controllers;

[ApiController]
[Route("api/[controller]")]
public class InventoryController : ControllerBase
{
    private IInventoryService inventoryService;

    public InventoryController(IInventoryService inventoryService)
    {
        this.inventoryService = inventoryService;
    }

    [HttpGet("all")]
    public async Task<IEnumerable<InventoryItemResponse>> GetAll()
    {
        return await inventoryService.GetAllAsync();
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var item = await inventoryService.Get(id);
        if (item == null) return NotFound();
        return Ok(item);
    }

    [HttpPost]
    public async Task<IActionResult> Add([FromBody] InventoryItemRequest inventoryItem)
    {
        var response = await inventoryService.AddAsync(inventoryItem);
        return CreatedAtAction(nameof(Get), new { id = response.Id }, response);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var item = await inventoryService.Get(id);
        if (item == null) return NotFound();
        await inventoryService.DeleteAsync(id);
        return Ok();
    }
}