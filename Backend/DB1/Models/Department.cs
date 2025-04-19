namespace DB1.Models;

public class Department 
{
    public int ID { get; set; }
    public string Name { get; set; } = string.Empty;

    // Navigation property
    // This property is used to establish a relationship with the Employee entity
    // and allows for easy access to the related employee information.
    // It is not mapped to the database, but is used in the application logic.
    public List<Employee> Employees { get; set; } = new List<Employee>();
}