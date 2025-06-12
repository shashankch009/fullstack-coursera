
using System.ComponentModel.DataAnnotations;

namespace LogiTrack.Models.DB;

public class InventoryItem
{
    [Key]
    public int ItemId { get; set; }

    [Required]
    [StringLength(200)]
    public string Name { get; set; }

    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1.")]
    public int Quantity { get; set; }

    [Required]
    public string Location { get; set; }

    public void DisplayInfo()
    {
        Console.WriteLine($"Item: {Name} | Quantity: {Quantity} | Location: {Location}");
    }
}