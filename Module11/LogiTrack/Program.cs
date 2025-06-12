using LogiTrack.DB;
using LogiTrack.Models.DB;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddDbContext<LogiTrackContext>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapGet("/hello", () =>
{
    return "Hello there!";
});

app.MapGet("/test", (LogiTrackContext dbContext) =>
{
    // Add test inventory item if none exist
    if (!dbContext.InventoryItems.Any())
    {
        dbContext.InventoryItems.Add(new InventoryItem
        {
            Name = "Pallet Jack",
            Quantity = 12,
            Location = "Warehouse A"
        });

        dbContext.SaveChanges();
    }

    // Retrieve and print inventory to confirm
    var items = dbContext.InventoryItems.ToList();
    foreach (var item in items)
    {
        item.DisplayInfo(); // Should print: Item: Pallet Jack | Quantity: 12 | Location: Warehouse A
    }
});

app.Run();