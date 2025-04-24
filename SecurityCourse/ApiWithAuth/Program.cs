using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<IdentityDbContext>(Options => 
    Options.UseInMemoryDatabase("ApiWithAuthDb"));

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<IdentityDbContext>();

builder.Services.AddAuthentication();
builder.Services.AddAuthorizationBuilder()
    .AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"))
    .AddPolicy("ITDepartmentOnly", policy => policy.RequireClaim("Department", "IT"));



//this will ensure that if user is not authenticated, calls to /api routes will return 401 status code instead of redirecting to login page
builder.Services.ConfigureApplicationCookie(options => {
    options.Events.OnRedirectToLogin = context => {
        Console.WriteLine("Redirecting to login page...");
        if(context.Response.StatusCode == StatusCodes.Status200OK) {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return Task.CompletedTask;
        }
        else {
            context.Response.Redirect(context.RedirectUri);
            return Task.CompletedTask;
        }
    };
});

var app = builder.Build();

string [] roles = new string[] { "Admin", "User"};

app.MapGet("/", () => "Hello World!");

app.MapGet("/account/admin", () => "Admin only route")
    .RequireAuthorization("AdminOnly");

app.MapGet("/account/user", () => "User only route")
    .RequireAuthorization();  

app.MapGet("/account/user-it", () => "User claim route for IT department")
    .RequireAuthorization("ITDepartmentOnly");

//without this call, if authorisation fails for above routes 
//which require authorisation, above calls will return 404 not found, which is not valid response
//that happens beacuse when authorisation fails, calls are redirected to /account/login page
//so we need to add this api route. 
app.MapGet("/account/login", () => "Login route");

app.MapGet("/public", () => "Public route");

app.MapPost("/account/roles", async (RoleManager<IdentityRole> RoleManager) => {

    foreach(var role in roles) {
        if(!await RoleManager.RoleExistsAsync(role)) {
            await RoleManager.CreateAsync(new IdentityRole(role));
        }
    }
    return Results.Ok("Roles created successfully");
});

app.MapPost("/account/user/{id}/assign-role/{role}", async (int id, string role, UserManager<IdentityUser> UserManager) => {
    var user = await UserManager.FindByIdAsync(id.ToString());
    if(user == null) {
        return Results.NotFound("User not found");
    }
    var result = await UserManager.AddToRoleAsync(user, role);
    if(result.Succeeded) {
        return Results.Ok("Role assigned successfully");
    }
    return Results.BadRequest("Failed to assign role");
});

//below ones for testing only, to create user and assign roles

app.MapPost("/account/register-test", async (UserManager<IdentityUser> userManager)  => 
{
    var testUser = new IdentityUser { UserName = "testuser", Email = "testuser@gmail.com"};
    var result = await userManager.CreateAsync(testUser, "Test@123");
    if(result != null && result.Succeeded) 
    {
        Console.WriteLine($"User created successfully. ID : {testUser.Id}");
        return Results.Ok($"User created successfully with Id: {testUser.Id}");
    }
    else 
    {
        return Results.BadRequest("Failed to create user");
    }
});

app.MapPost("/account/assign-admin-test", async (UserManager<IdentityUser> userManager) => {
    var user = await userManager.FindByEmailAsync("testuser@gmail.com");
    if(user == null) 
    {
        return Results.NotFound("User not found");
    }
    var result = await userManager.AddToRoleAsync(user, "Admin");
    if(result.Succeeded) 
    {
        return Results.Ok("User assigned to Admin role successfully");
    }
    else 
    {
        return Results.BadRequest("Failed to assign user to Admin role");
    }
});

app.MapPost("/account/add-claim-test", async (UserManager<IdentityUser> userManager) => 
{
    var user = await userManager.FindByEmailAsync("testuser@gmail.com");
    if(user == null) 
    {
        return Results.NotFound("User not found");
    }
    var claim = new Claim("Department", "IT"); //key, value pair
    var result = await userManager.AddClaimAsync(user, claim);
    if(result.Succeeded) 
    {
        return Results.Ok("Claim added successfully");
    }
    else 
    {
        return Results.BadRequest("Failed to add claim to user");
    }
});

app.MapPost("/account/login-test", async (SignInManager<IdentityUser> signInManager) => 
{
    var result = await signInManager.PasswordSignInAsync("testuser", "Test@123", isPersistent: false, lockoutOnFailure: false);
    if(result.Succeeded) 
    {
        return Results.Ok("User logged in successfully");
    }
    else 
    {
        return Results.BadRequest("Failed to login user");
    }
});

app.Run();
