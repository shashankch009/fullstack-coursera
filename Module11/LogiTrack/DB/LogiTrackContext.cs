
using Microsoft.EntityFrameworkCore;
using LogiTrack.Models.DB;

namespace LogiTrack.DB;

public class LogiTrackContext : DbContext
{
    public DbSet<InventoryItem> InventoryItems { get; set; }
    public DbSet<Order> Orders { get; set; }

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
}
