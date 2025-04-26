var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddCors(options => 
{
    options.AddDefaultPolicy(builder => {
        builder.AllowAnyHeader().AllowAnyMethod().SetIsOriginAllowed(_ => true);
    });
});

var app = builder.Build();

app.UseCors();
app.UseRouting();
app.MapControllers();

app.UseHttpsRedirection();

app.Run();

