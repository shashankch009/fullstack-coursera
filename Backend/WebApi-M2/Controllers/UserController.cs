using Microsoft.AspNetCore.Mvc;
using System;
using WebApi_M2.Models;
using WebApi_M2.Services;

namespace WebApi_M2.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly UserService userService;
    public UserController(UserService userService)
    {
        this.userService = userService;
    }

    [HttpGet]
    public IEnumerable<User> Read()
    {
        return userService.GetAllUsers();
    }

    [HttpGet("{id}")]
    public User Read(int id)
    {
        return userService.GetUserById(id);
    }

    [HttpPost]
    public IActionResult Create([FromBody] User user)
    {
        var newUser = userService.AddUser(user);
        return Ok(newUser);
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, [FromBody] User user) 
    {
        var updatedUser = userService.UpdateUser(id, user);
        if (updatedUser != null) 
        {
            return Ok(updatedUser);
        }
        return NotFound();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        userService.DeleteUser(id);
        return NoContent();
    }
}
