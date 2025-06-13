
using LogiTrack.Models.DB;

public class OrderInventoryItem
{
    public int OrderId { get; set; }

    //navigation property 
    public Order Order { get; set; }

    public int InventoryItemId { get; set; }

    //navigation property 
    public InventoryItem InventoryItem { get; set; } 

    public int Quantity { get; set; }
}