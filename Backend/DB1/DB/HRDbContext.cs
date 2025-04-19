using DB1.Models;
using Microsoft.EntityFrameworkCore;

namespace DB1.DB;

public class HRDbContext : DbContext
{
    private readonly string _connectionString;

    public DbSet<Employee> Employees { get; set; } = null!;
    
    public DbSet<Department> Departments { get; set; } = null!;

    public HRDbContext(DbContextOptions<HRDbContext> options, IConfiguration configuration) : base(options)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseMySql(_connectionString, new MySqlServerVersion(new Version(9, 3, 0)));
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Employee>(entity => 
        {
            entity.HasKey(e => e.ID);
            entity.Property(e => e.FirstName).IsRequired().HasMaxLength(50);
            entity.Property(e => e.LastName).HasMaxLength(50);
            entity.Property(e => e.HireDate).IsRequired();

            // Configure relationship with Department
            entity.HasOne(e => e.Department)
                  .WithMany(d => d.Employees)
                  .HasForeignKey(e => e.DepartmentID)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Department>(entity => 
        {
            entity.HasKey(d => d.ID);
            entity.Property(d => d.Name).IsRequired().HasMaxLength(100);
        });
    }
}