
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ApiServer.Data;

public class AppIdentityDbContext : IdentityDbContext
{
    private readonly string connectionString;

    public AppIdentityDbContext(DbContextOptions<AppIdentityDbContext> options, IConfiguration configuration)
        : base(options)
    {
        connectionString = configuration.GetConnectionString("DefaultConnection") 
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if(!optionsBuilder.IsConfigured) 
        {
            optionsBuilder.UseMySql(connectionString, new MySqlServerVersion(new Version(9, 3, 0)));
        }
    }
}