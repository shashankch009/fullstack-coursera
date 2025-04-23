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

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.MapGet("/admin", () => "Admin only route");

app.MapGet("/user", () => "User only route");  

app.MapGet("user-it", () => "User claim route for IT department");

app.MapGet("/public", () => "Public route");


app.Run();
