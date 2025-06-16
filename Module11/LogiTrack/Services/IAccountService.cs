
using LogiTrack.Models.Api;
using Microsoft.AspNetCore.Identity;

namespace LogiTrack.Services;

public interface IAccountService 
{
    Task<IdentityResult> RegisterAsync(RegisterInfo registerInfo);

    Task<SignInResult> LoginAsync(LoginInfo loginInfo);

    Task LogoutAsync();
}