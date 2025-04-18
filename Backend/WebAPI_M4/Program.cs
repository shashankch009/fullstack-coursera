var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer(); // Add support for API documentation
builder.Services.AddSwaggerGen(); // Add Swagger for API documentation

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenLocalhost(5238); // HTTP only
});


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

// Middleware to log security events if response status indicates an issue
app.Use(async (context, next) =>
{
    await next(); // Run the next middleware first

    if (context.Response.StatusCode >= 400)
    {
        Console.WriteLine($"Security Event: {context.Request.Path} - Status Code: {context.Response.StatusCode}");
    }
});

// Simulated HTTPS Enforcement Middleware
app.Use(async (context, next) =>
{
    // Check for a query parameter to simulate HTTPS enforcement (e.g., "?secure=true")
    if (context.Request.Query["secure"] != "true")
    {
        context.Response.StatusCode = 400;
        await context.Response.WriteAsync("Simulated HTTPS Required");
        return;
    }

    await next();
});

// Middleware for simulated authentication and secure cookies
app.Use(async (context, next) =>
{
    // Simulate authentication with a query parameter (e.g., "?authenticated=true")
    var isAuthenticated = context.Request.Query["authenticated"] == "true";
    if (!isAuthenticated)
    {
        if (!context.Response.HasStarted)
        {
            context.Response.StatusCode = 403;
            await context.Response.WriteAsync("Access Denied");
        }
        return;
    }

    context.Response.Cookies.Append("SecureCookie", "SecureData", new CookieOptions
    {
        HttpOnly = true,
        Secure = true
    });

    await next();
});

// Middleware for short-circuiting unauthorized access
app.Use(async (context, next) =>
{
    if (context.Request.Path == "/unauthorized")
    {
        if (!context.Response.HasStarted)
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync("Unauthorized Access");
        }
        return; // Exit middleware pipeline early if unauthorized
    }
    await next();
});

// Middleware for input validation
app.Use(async (context, next) =>
{
    var input = context.Request.Query["input"];
    if (!IsValidInput(input))
    {
        if (!context.Response.HasStarted)
        {
            context.Response.StatusCode = 400;
            await context.Response.WriteAsync("Invalid Input");
        }
        return;
    }

    await next();
});

// Helper method for input validation
static bool IsValidInput(string input)
{
    // Checks for any unsafe characters or patterns, including "<script>"
    return string.IsNullOrEmpty(input) || (input.All(char.IsLetterOrDigit) && !input.Contains("<script>"));
}

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
