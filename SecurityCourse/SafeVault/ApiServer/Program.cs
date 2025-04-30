using ApiServer.Data;
using ApiServer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options => {
    options.AddDefaultPolicy(builder => {
        builder.AllowAnyHeader().AllowAnyMethod().AllowCredentials().SetIsOriginAllowed(origin => true);
    });
});

builder.Services.AddControllers();

builder.Services.AddDbContext<AppIdentityDbContext>();

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<AppIdentityDbContext>();

builder.Services.AddScoped<IAccountService, AccountService>();

builder.Services.AddAuthorization();

var app = builder.Build();
app.UseCors();

// Add Authentication and Authorization Middleware
app.UseAuthentication(); 
app.UseAuthorization(); 

app.MapControllers();

app.MapGet("/", () => "Hello World!");

app.Run();
