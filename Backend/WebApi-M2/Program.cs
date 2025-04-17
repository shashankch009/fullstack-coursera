using WebApi_M2.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers(); // Added to register controllers

builder.Services.AddSingleton<WeatherService>(); // Register WeatherService as a singleton
builder.Services.AddSingleton<UserService>(); 
builder.Services.AddSingleton<ProductService>(); 

var app = builder.Build();

app.Use(async (context, next) =>
{
    // context is of type Microsoft.AspNetCore.Http.HttpContext
    // next is of type System.Func<Microsoft.AspNetCore.Http.HttpContext, System.Threading.Tasks.Task>
    // Do work that can write to the Response.
    Console.WriteLine("logic before");
    await next.Invoke();
    // Do logging or other work that doesn't write to the Response.
    Console.WriteLine("logic after");
});

app.MapGet("/", () => "Hello World!");

app.MapControllers(); // Added to map controller routes

app.Run();
