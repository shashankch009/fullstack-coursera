
using LogiTrack.DB;
using LogiTrack.Models;
using LogiTrack.Models.Api;
using LogiTrack.Models.DB;
using Microsoft.EntityFrameworkCore;

namespace LogiTrack.Services;

public class InventoryService : IInventoryService
{
    private LogiTrackContext dbContext;

    public InventoryService(LogiTrackContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<InventoryItemResponse?> Get(int id)
    {
        var dbItem = await dbContext.InventoryItems.FindAsync(id);
        if (dbItem == null) return null;

        return ApiDbModelConverter.ToApiResponse(dbItem);
    }

    public async Task<IEnumerable<InventoryItemResponse>> GetAllAsync()
    {
        return await dbContext.InventoryItems.Select(item => ApiDbModelConverter.ToApiResponse(item)).ToListAsync();
    }

    public async Task<InventoryItemResponse> AddAsync(InventoryItemRequest request)
    {
        var dbItem = new InventoryItem
        {
            Name = request.Name,
            Quantity = request.Quantity,
            Location = request.Location
        };
        await dbContext.InventoryItems.AddAsync(dbItem);
        await dbContext.SaveChangesAsync(); // dbItem.ItemId is now set

        return ApiDbModelConverter.ToApiResponse(dbItem);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var dbItem = await dbContext.InventoryItems.FindAsync(id);
        if (dbItem != null)
        {
            dbContext.InventoryItems.Remove(dbItem);
            await dbContext.SaveChangesAsync();
        }
        return true;
    }
}