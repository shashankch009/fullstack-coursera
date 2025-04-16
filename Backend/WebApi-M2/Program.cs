var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers(); // Added to register controllers

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.MapControllers(); // Added to map controller routes

app.Run();
