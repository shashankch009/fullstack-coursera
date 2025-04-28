
using ApiServer.Models.Api;
using Microsoft.AspNetCore.Identity;

namespace ApiServer.Services;

public interface IAccountService 
{
    Task<IdentityResult> RegisterAsync(RegisterInfo registerInfo);
    
    Task<SignInResult> LoginAsync(LoginInfo loginInfo);
   
    Task LogoutAsync();

    bool VerifyUser(string username, string password);
}