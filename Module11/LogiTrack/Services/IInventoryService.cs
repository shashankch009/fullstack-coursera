
using LogiTrack.Models.Api;

namespace LogiTrack.Services;

public interface IInventoryService
{
    Task<InventoryItemResponse?> Get(int id);
    
    Task<IEnumerable<InventoryItemResponse>> GetAllAsync();

    Task<InventoryItemResponse> AddAsync(InventoryItemRequest request);

    Task<bool> DeleteAsync(int id);
}