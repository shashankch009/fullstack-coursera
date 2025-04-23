using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<IdentityDbContext>(Options => 
    Options.UseInMemoryDatabase("ApiWithAuthDb"));

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<IdentityDbContext>();

builder.Services.AddAuthentication();
builder.Services.AddAuthorizationBuilder();


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

app.MapGet("/", () => "Hello World!");

app.MapGet("/account/admin", () => "Admin only route")
    .RequireAuthorization();

app.MapGet("/account/user", () => "User only route")
    .RequireAuthorization();  

app.MapGet("/account/user-it", () => "User claim route for IT department")
    .RequireAuthorization();

//without this call, if authorisation fails for above routes 
//which require authorisation, above calls will return 404 not found, which is not valid response
//that happens beacuse when authorisation fails, calls are redirected to /account/login page
//so we need to add this api route. 
app.MapGet("/account/login", () => "Login route");

app.MapGet("/public", () => "Public route");


app.Run();
