var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer(); // Add support for API documentation
builder.Services.AddSwaggerGen(); // Add Swagger for API documentation


var app = builder.Build();

//Configure Exception Handling
if(!app.Environment.IsDevelopment()) {
    app.UseExceptionHandler("/error"); // Use a custom error handling endpoint
}
else {
    app.UseDeveloperExceptionPage(); // Use the developer exception page in development
}

if(app.Environment.IsDevelopment()) {
    app.UseSwagger(); // Enable Swagger in development
    app.UseSwaggerUI(); // Enable Swagger UI in development
}

app.Use(async (context, next) => {
    var startTime = DateTime.UtcNow;
    await next.Invoke(); // Call the next middleware in the pipeline
    var duration = DateTime.UtcNow - startTime;
    Console.WriteLine($"Request duration: {duration.TotalMilliseconds} ms");
});

// Middleware for asynchronous processing
app.Use(async (context, next) =>
{
    await Task.Delay(100); // Simulate async operation
    if (!context.Response.HasStarted)
    {
        await context.Response.WriteAsync("Processed Asynchronously\n");
    }
    await next();
});


app.UseWhen(context => context.Request.Method != "GET", appBuilder => {
    appBuilder.Use(async (context, next) => {
        Console.WriteLine("Non-GET request detected.");
        await next.Invoke(); // Call the next middleware in the pipeline
        //can do checks like - if authenticated 
        //if not authenticated, return 401 Unauthorized
        //if authenticated, call next middleware
        //e.g.
        //if (!context.User.Identity.IsAuthenticated) {
        //     context.Response.StatusCode = 401; // Unauthorized
        //     await context.Response.WriteAsync("Unauthorized access.");
    });
});

// Final Response Middleware
app.Run(async (context) =>
{
    if (!context.Response.HasStarted)
    {
        await context.Response.WriteAsync("Final Response from Application\n");
    }
});

app.MapGet("/", () => {
    Console.WriteLine("Root endpoint hit.");
    return Results.Text("I am Root!");
});

app.Run();
