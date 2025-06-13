
using LogiTrack.Models.Api;

namespace LogiTrack.Services;

public interface IOrderService
{
    Task<OrderResponse?> Get(int id);
    
    Task<IEnumerable<OrderResponse>> GetAllAsync();

    Task<OrderResponse> AddAsync(OrderRequest request);

    Task<bool> DeleteAsync(int id);
}