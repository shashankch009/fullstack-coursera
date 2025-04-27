using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<JwtService>();

var secretKey = builder.Configuration["Authorization:JwtSecurityKey"];
builder.Services.AddAuthentication().AddJwtBearer(options => 
{
    options.TokenValidationParameters = new TokenValidationParameters {
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)), 
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidIssuer = builder.Configuration["Authorization:JwtIssuer"], // "ApiAuthJWTApp"
        ValidAudience = builder.Configuration["Authorization:JwtAudience"] // "ApiWithJwtAuth"
    };
});

builder.Services.AddAuthorization();

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.MapPost("/login", (LoginInfo loginInfo, JwtService jwtService) => {
    Console.WriteLine($"received login info - user: {loginInfo.UserName}, password: {loginInfo.Password}");
     // Hardcoded username and password for testing
    const string testUserName = "testuser";
    const string testPassword = "password123";
    const string role = "user";

    if(loginInfo.UserName != testUserName || loginInfo.Password != testPassword)
    {
        return Results.Unauthorized();
    }

    var tokenString = jwtService.Generate(loginInfo.UserName, role);

    return Results.Ok(new { token = tokenString });
});

app.MapGet("/secure", () => "Secure route").RequireAuthorization();

app.Run();

class LoginInfo 
{
    public required string UserName {get;set;}
    public required string Password {get;set;}
}
