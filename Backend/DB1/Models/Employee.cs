
using System.ComponentModel.DataAnnotations;

namespace DB1.Models;

public class Employee
{
    [Key]
    public int ID { get; set; }

    [Required]
    [StringLength(50)]
    public string FirstName { get; set; } = string.Empty;

    [StringLength(50)]
    public string LastName { get; set; } = string.Empty;

    public DateTime HireDate { get; set; }

    public int DepartmentID { get; set; }

    // Navigation property
    // This property is used to establish a relationship with the Department entity
    // and allows for easy access to the related department information.
    // It is not mapped to the database, but is used in the application logic.
    public Department? Department { get; set; }
}