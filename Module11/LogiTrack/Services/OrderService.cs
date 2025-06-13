
using LogiTrack.DB;
using LogiTrack.Models;
using LogiTrack.Models.Api;
using LogiTrack.Models.DB;
using Microsoft.EntityFrameworkCore;

namespace LogiTrack.Services;

public class OrderService : IOrderService
{
    private LogiTrackContext dbContext;

    public OrderService(LogiTrackContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<OrderResponse?> Get(int id)
    {
        var dbItem = await dbContext.Orders
            .Include(o => o.Customer)
            .Include(o => o.OrderInventoryItems)
                .ThenInclude(oi => oi.InventoryItem)
            .FirstOrDefaultAsync(o => o.OrderId == id);

        if (dbItem == null) return null;

        return ApiDbModelConverter.ToApiResponse(dbItem);
    }

    public async Task<IEnumerable<OrderResponse>> GetAllAsync()
    {
        return await dbContext.Orders
            .Include(o => o.Customer)
            .Include(o => o.OrderInventoryItems)
            .ThenInclude(oi => oi.InventoryItem)
            .Select(item => ApiDbModelConverter.ToApiResponse(item))
            .ToListAsync();
    }

    public async Task<OrderResponse> AddAsync(OrderRequest request)
    {
        var dbItem = new Order
        {
            CustomerId = request.CustomerId,
            DatePlaced = DateTime.UtcNow
        };
        await dbContext.Orders.AddAsync(dbItem);
        await dbContext.SaveChangesAsync(); // dbItem.ItemId is now set

        //InventoryItemIds
        foreach (var iid in request.InventoryItemIds)
        {
            var oi = new OrderInventoryItem
            {
                OrderId = dbItem.OrderId,
                InventoryItemId = iid,
                Quantity = 1 //TODO 
            };
            await dbContext.OrderInventoryItems.AddAsync(oi);
        }
        await dbContext.SaveChangesAsync();

        // Reload the order with navigation properties
        var fullOrder = await dbContext.Orders
            .Include(o => o.Customer)
            .Include(o => o.OrderInventoryItems)
                .ThenInclude(oi => oi.InventoryItem)
            .FirstOrDefaultAsync(o => o.OrderId == dbItem.OrderId);

        return ApiDbModelConverter.ToApiResponse(dbItem);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var dbItem = await dbContext.Orders.FindAsync(id);
        if (dbItem != null)
        {
            dbContext.Orders.Remove(dbItem);
            await dbContext.SaveChangesAsync();
        }
        return true;
    }
}
