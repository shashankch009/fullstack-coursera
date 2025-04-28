using ApiServer.Data;
using ApiServer.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options => {
    options.AddDefaultPolicy(builder => {
        builder.AllowAnyHeader().AllowAnyMethod().AllowCredentials().SetIsOriginAllowed(origin => true);
    });
});

builder.Services.AddControllers();

builder.Services.AddDbContext<AppIdentityDbContext>(options => 
    options.UseInMemoryDatabase("AuthAppIdentity"));

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<AppIdentityDbContext>();

builder.Services.AddScoped<IAccountService, AccountService>();

var app = builder.Build();
app.UseCors();

app.MapControllers();

app.MapGet("/", () => "Hello World!");

app.Run();
