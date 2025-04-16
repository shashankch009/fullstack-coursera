using WebApi_M2.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers(); // Added to register controllers
builder.Services.AddSingleton<WeatherService>(); // Register WeatherService as a singleton
var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.MapControllers(); // Added to map controller routes

app.Run();
