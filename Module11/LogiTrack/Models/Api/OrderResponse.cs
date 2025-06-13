
namespace LogiTrack.Models.Api;

public class OrderResponse
{
    public int Id { get; set; }

    public CustomerResponse Customer { get; set; }

    public DateTime DatePlaced { get; set; }

    public List<InventoryItemResponse> Items { get; set; } = new List<InventoryItemResponse>();
}