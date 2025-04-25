
using Microsoft.AspNetCore.Identity;

namespace ApiServer.Services;

public class AccountService 
{
    private readonly IConfiguration configuration;
    private readonly UserManager<IdentityUser> userManager;
    private readonly SignInManager<IdentityUser> signInManager;
    private readonly RoleManager<IdentityRole> roleManager;

    public AccountService(IConfiguration configuration, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, RoleManager<IdentityRole> roleManager)
    {
        this.configuration = configuration;
        this.userManager = userManager;
        this.signInManager = signInManager;
        this.roleManager = roleManager;
    }
}