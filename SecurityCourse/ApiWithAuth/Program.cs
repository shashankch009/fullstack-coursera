var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.MapGet("/admin", () => "Admin only route");

app.MapGet("/user", () => "User only route");  

app.MapGet("user-it", () => "User claim route for IT department");

app.MapGet("/public", () => "Public route");


app.Run();
