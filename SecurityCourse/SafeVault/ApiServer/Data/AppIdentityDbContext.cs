
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;

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

    public bool VerifyUser(string username, string password)
    {
        string query = "SELECT * FROM AspNetUsers WHERE UserName = @username";

        var user = this.Users.FromSqlRaw(query, new MySqlParameter("@USERNAME", username)).FirstOrDefault();

        if (user == null || user.PasswordHash == null)
        {
            return false; // User not found
        }

        var result = new PasswordHasher<IdentityUser>().VerifyHashedPassword(user, user.PasswordHash, password);
        return result == PasswordVerificationResult.Success;
    }
}