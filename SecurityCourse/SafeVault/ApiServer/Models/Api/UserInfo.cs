
namespace ApiServer.Models.Api;

public class UserInfo
{
    public required string UserName { get; set; }
    public required string Email { get; set; }
    public IList<string> Roles { get; set; }
}