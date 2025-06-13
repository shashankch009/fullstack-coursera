
using System.ComponentModel.DataAnnotations;

namespace LogiTrack.Models.DB;

public class Order
{
    [Key]
    public int OrderId { get; set; }

    [Required]
    public int CustomerId { get; set; }

    //navigation property 
    public Customer Customer { get; set; }

    [Required]
    public DateTime DatePlaced { get; set; }

    // Navigation property for many-to-many relationship
    public List<OrderInventoryItem> OrderInventoryItems { get; set; }
}