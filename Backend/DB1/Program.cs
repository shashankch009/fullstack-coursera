using DB1.DB;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<HRDbContext>();
builder.Services.AddControllers();
builder.Services.AddScoped<HRDbService>();

var app = builder.Build();

app.MapControllers();

app.MapGet("/", () => "Hello World!");

app.Run();
