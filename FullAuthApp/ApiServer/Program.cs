var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options => {
    options.AddDefaultPolicy(builder => {
        builder.AllowAnyHeader().AllowAnyMethod().AllowCredentials().SetIsOriginAllowed(origin => true);
    });
});

builder.Services.AddControllers();

var app = builder.Build();
app.UseCors();

app.MapControllers();

app.MapGet("/", () => "Hello World!");

app.Run();
