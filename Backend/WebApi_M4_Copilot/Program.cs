using System.Collections.Concurrent;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

var users = new ConcurrentDictionary<int, User>();

app.MapGet("/", () => "Hello World!");

// Create
app.MapPost("/users", (User user) => {
    user.Id = users.Count > 0 ? users.Keys.Max() + 1 : 1;
    if (!users.TryAdd(user.Id, user))
    {
        return Results.Conflict("Failed to add user.");
    }
    return Results.Created($"/users/{user.Id}", user);
});

// Read all
app.MapGet("/users", () => users.Values);

// Read by ID
app.MapGet("/users/{id}", (int id) => {
    return users.TryGetValue(id, out var user) ? Results.Ok(user) : Results.NotFound();
});

// Update
app.MapPut("/users/{id}", (int id, User updatedUser) => {
    if (!users.ContainsKey(id)) return Results.NotFound();

    updatedUser.Id = id; // Ensure the ID remains the same
    users[id] = updatedUser;
    return Results.NoContent();
});

// Delete
app.MapDelete("/users/{id}", (int id) => {
    if (!users.TryRemove(id, out _)) return Results.NotFound();

    return Results.NoContent();
});

app.Run();

public record User
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public int Age { get; set; }
}
