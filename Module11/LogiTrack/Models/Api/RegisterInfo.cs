
namespace LogiTrack.Models.Api;

public class RegisterInfo
{
    public required string FullName { get; set; }
    
    public required string Email { get; set; }

    public required string Password { get; set; }

    public string Role { get; set; } = UserRole.User;
}