
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(options => 
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme; //"Cookies"
    options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme; //"Google"
})
.AddCookie()
.AddGoogle(options  => 
{
    options.ClientId = builder.Configuration["Authorization:Google:ClientId"];
    options.ClientSecret = builder.Configuration["Authorization:Google:ClientSecret"];
    options.CallbackPath = "/signin-google"; // The path to redirect to after sign-in
        //this is default as well.  
    options.Scope.Add("email"); // Request access to the user's email address
    options.Scope.Add("profile"); // Request access to the user's profile information
});

//call to /signin-google is handled internally by Google middleware
// and everything is handled interally and cookies are set. 
//what happens internally, after user logs in with Google?
// 1. Redirect to /signin-google, with authorization code
// 2. Google middleware intercepts the call to /signin-google and exchanges the authorization code for an access token
// 3. Google middleware retrieves user information using the access token
// 4.  middleware signs the user in using the configured authentication scheme (e.g., cookies here)
// 5. so in case of cookie method, session cookies are set. 
// 6. redirect to the original URL or the default URL specified in the authentication options.
// So note that we don't need to handle the /signin-google endpoint manually in our code. 
// Even if we create a endpoint /signin-google, it will not be called.

builder.Services.AddAuthorization();

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/", () => "Hello World!");

app.MapGet("/home", () => "Hello User!").RequireAuthorization();

app.MapPost("/logout", async context => {
    Console.WriteLine("Logging out...");
    await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    context.Response.Redirect("/");
});

app.MapGet("/logout", async context => {
    Console.WriteLine("Logging out...");
    await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    context.Response.Redirect("/");
});

// Custom implementation of /signin-google endpoint
app.MapGet("/signin-google", async context =>
{
    Console.WriteLine("Sign in with Google callback triggered");

    // Log query parameters (e.g., authorization code)
    var query = context.Request.Query;
    foreach (var param in query)
    {
        Console.WriteLine($"{param.Key}: {param.Value}");
    }

    // Let the Google middleware handle the request
    await context.ChallengeAsync(GoogleDefaults.AuthenticationScheme, new AuthenticationProperties
    {
        RedirectUri = "/" // Redirect to the home page after successful login
    });
});

app.Run();
