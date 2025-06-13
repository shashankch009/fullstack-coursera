namespace LogiTrack.Models.Api;

public class InventoryItemResponse
{
    public int Id { get; set; }

    public string Name { get; set; }

    public int Quantity { get; set; }

    public string Location { get; set; }
}