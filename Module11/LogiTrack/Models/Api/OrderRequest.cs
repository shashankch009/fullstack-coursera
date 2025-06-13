
namespace LogiTrack.Models.Api;

public class OrderRequest
{
    public int CustomerId { get; set; }

    public List<int> InventoryItemIds { get; set; } = new List<int>();
}