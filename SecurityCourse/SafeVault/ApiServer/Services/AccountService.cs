
using ApiServer.Data;
using ApiServer.Models.Api;
using Microsoft.AspNetCore.Identity;

namespace ApiServer.Services;

public class AccountService  : IAccountService
{
    private readonly IConfiguration configuration;
    private readonly UserManager<IdentityUser> userManager;
    private readonly SignInManager<IdentityUser> signInManager;
    private readonly RoleManager<IdentityRole> roleManager;

    private readonly AppIdentityDbContext dbContext;

    public AccountService(IConfiguration configuration, 
        UserManager<IdentityUser> userManager, 
        SignInManager<IdentityUser> signInManager, 
        RoleManager<IdentityRole> roleManager,
        AppIdentityDbContext dbContext)
    {
        this.configuration = configuration;
        this.userManager = userManager;
        this.signInManager = signInManager;
        this.roleManager = roleManager;
        this.dbContext = dbContext;
    }

    public async Task<IdentityResult> RegisterAsync(RegisterInfo registerInfo) 
    {
        var user = new IdentityUser 
        {
            UserName = registerInfo.Email, 
            Email = registerInfo.Email,
        };
        var userFound = await userManager.FindByEmailAsync(user.Email);
        if(userFound != null)
        {
            Console.WriteLine("user already exists with same email");
            return IdentityResult.Failed(new IdentityError{
                Code = "Already exists",
                Description = "Account with this email ID already exists."
            });
        }
        
        Console.WriteLine("creating new account");
        var result = await userManager.CreateAsync(user, registerInfo.Password);
        if(!result.Succeeded) 
        {
            return result;
        }

        if(!await roleManager.RoleExistsAsync(registerInfo.Role)) 
        {
            Console.WriteLine("creating role : " + registerInfo.Role);
            await roleManager.CreateAsync(new IdentityRole(registerInfo.Role));
            Console.WriteLine("Role created");
        }

        await userManager.AddToRoleAsync(user, registerInfo.Role);
        Console.WriteLine("User added to the role");
        if(await userManager.IsInRoleAsync(user, registerInfo.Role))
        {
            Console.WriteLine("User successfully assigned the role");
            return result;
        }
        else 
        {
            result = IdentityResult.Failed(new IdentityError 
            {
                Code = "RoleError", 
                Description = $"User is not in role {registerInfo.Role}."
            });
            return result;
        }
    }

    public async Task<SignInResult> LoginAsync(LoginInfo loginInfo)
    {
        var result = await signInManager.PasswordSignInAsync(loginInfo.UserName, loginInfo.Password, isPersistent: false, lockoutOnFailure: false);
        if (result.Succeeded)
        {
            Console.WriteLine("User logged in successfully");
            return result;
        }
        else
        {
            return SignInResult.Failed;
        }
    }

    public bool VerifyUser(string username, string password)
    {
        return dbContext.VerifyUser(username, password);
    }

    public async Task LogoutAsync()
    {
        await signInManager.SignOutAsync();
        Console.WriteLine("User logged out!");
    }
}