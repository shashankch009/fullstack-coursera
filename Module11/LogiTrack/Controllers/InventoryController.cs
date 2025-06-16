using LogiTrack.Models.Api;
using Microsoft.AspNetCore.Mvc;
using LogiTrack.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Caching.Memory;

namespace LogiTrack.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Policy = "AdminOnly")]
public class InventoryController : ControllerBase
{
    private IInventoryService inventoryService;

    private IMemoryCache memoryCache;

    public InventoryController(IInventoryService inventoryService, IMemoryCache memoryCache)
    {
        this.inventoryService = inventoryService;
        this.memoryCache = memoryCache;
    }

    [HttpGet("all")]
    public async Task<IEnumerable<InventoryItemResponse>> GetAll()
    {
        var cacheKey = "inventory-all";
        IEnumerable<InventoryItemResponse> result;
        if (!memoryCache.TryGetValue(cacheKey, out result))
        {
            result = await inventoryService.GetAllAsync();
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromSeconds(30));
            memoryCache.Set(cacheKey, result, cacheEntryOptions);
            Console.WriteLine("set inventory in cache");
        }
        else
        {
            Console.WriteLine("found inventory in cache");
        }
        return result;
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