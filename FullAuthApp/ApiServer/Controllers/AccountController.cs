
using ApiServer.Services;
using Microsoft.AspNetCore.Mvc;

namespace ApiServer.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountController : ControllerBase 
{
    private readonly AccountService accountService;

    public AccountController(AccountService accountService)
    {
        this.accountService = accountService;
    }
    
}