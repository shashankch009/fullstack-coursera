
using ApiServer.Models.Api;
using ApiServer.Services;
using Microsoft.AspNetCore.Mvc;

namespace ApiServer.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountController : ControllerBase 
{
    private readonly IAccountService accountService;

    public AccountController(IAccountService accountService)
    {
        this.accountService = accountService;
    }


    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterInfo registerInfo)
    {
        if (registerInfo == null)
        {
            return BadRequest("Invalid registration information.");
        }

        var result = await accountService.RegisterAsync(registerInfo);
        if (result.Succeeded)
        {
            return Ok("User registered successfully.");
        }

        return BadRequest(result.Errors);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginInfo loginInfo)
    {
        if (loginInfo == null)
        {
            return BadRequest("Invalid login information.");
        }

        var result = await accountService.LoginAsync(loginInfo);
        if (result.Succeeded)
        {
            return Ok("User logged in successfully.");
        }

        return Unauthorized("Invalid username or password.");
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout() 
    {
        await accountService.LogoutAsync();
        return Ok("User logged out successfully.");
    }
}