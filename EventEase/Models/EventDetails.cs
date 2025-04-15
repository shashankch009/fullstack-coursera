using System.ComponentModel.DataAnnotations;

namespace EventEase.Models;
public class EventDetails
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Event name is required.")]
    [StringLength(100, ErrorMessage = "Event name cannot exceed 100 characters.")]
    public string Name { get; set; }

    [Required(ErrorMessage = "Event date is required.")]
    [DataType(DataType.Date)]
    public DateTime Date { get; set; }

    [Required(ErrorMessage = "Event location is required.")]
    [StringLength(200, ErrorMessage = "Event location cannot exceed 200 characters.")]
    public string Location { get; set; }
}