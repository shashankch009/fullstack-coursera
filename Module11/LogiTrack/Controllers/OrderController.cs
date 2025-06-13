
using LogiTrack.Models.Api;
using LogiTrack.Services;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]s")]
public class OrderController : ControllerBase
{
    private IOrderService orderService;

    public OrderController(IOrderService orderService)
    {
        this.orderService = orderService;
    }

    [HttpGet("all")]
    public async Task<IEnumerable<OrderResponse>> GetAll()
    {
        return await orderService.GetAllAsync();
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var order = await orderService.Get(id);
        if (order == null) return NotFound();

        return Ok(order);
    }

    [HttpPost]
    public async Task<IActionResult> Add([FromBody] OrderRequest order)
    {
        var response = await orderService.AddAsync(order);
        return CreatedAtAction(nameof(Get), new { id = response.Id }, response);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var item = await orderService.Get(id);
        if (item == null) return NotFound();
        await orderService.DeleteAsync(id);
        return Ok();
    }
}