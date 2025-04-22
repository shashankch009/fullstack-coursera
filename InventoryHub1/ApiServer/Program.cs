var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyHeader().AllowAnyMethod().AllowCredentials().SetIsOriginAllowed(_ => true);
    });
    //builder.WithOrigins("http://localhost:5017").AllowAnyHeader().AllowAnyMethod().AllowCredentials();
    //this url is of the web app / blazor app i.e client app 
});

var app = builder.Build();

app.UseCors();

app.MapGet("/", () => "Hello World!");

app.MapGet("/api/products", () =>
{
    return new[]
    {
        new 
        { 
            Id = 1, 
            Name = "Laptop", 
            Price = 1200.50, 
            Stock = 25,
            Category = new { Id = 101, Name = "Electronics" } 
        },
        new 
        { 
            Id = 2, 
            Name = "Headphones", 
            Price = 50.00, 
            Stock = 100,
            Category = new { Id = 102, Name = "Accessories" }
        }
    };
});

app.Run();