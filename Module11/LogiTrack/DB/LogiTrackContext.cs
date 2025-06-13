
using Microsoft.EntityFrameworkCore;
using LogiTrack.Models.DB;

namespace LogiTrack.DB;

public class LogiTrackContext : DbContext
{
    public DbSet<InventoryItem> InventoryItems { get; set; }
    
    public DbSet<Order> Orders { get; set; }

    public DbSet<OrderInventoryItem> OrderInventoryItems { get; set; }

    public LogiTrackContext(DbContextOptions<LogiTrackContext> options) : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlite("Data Source=logitrack.db");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<OrderInventoryItem>()
            .HasKey(oi => new { oi.OrderId, oi.InventoryItemId });

        modelBuilder.Entity<OrderInventoryItem>()
            .HasOne(oi => oi.Order)
            .WithMany(o => o.OrderInventoryItems)
            .HasForeignKey(oi => oi.OrderId);

        modelBuilder.Entity<OrderInventoryItem>()
            .HasOne(oi => oi.InventoryItem)
            .WithMany(i => i.OrderInventoryItems)
            .HasForeignKey(oi => oi.InventoryItemId);
    }
}
