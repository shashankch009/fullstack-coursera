
using System.ComponentModel.DataAnnotations;

namespace LogiTrack.Models.DB;

public class Customer
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }

    [StringLength(200)]
    public string Address { get; set; }
}