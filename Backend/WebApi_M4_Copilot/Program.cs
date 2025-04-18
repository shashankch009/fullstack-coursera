var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

var users = new List<User>();

app.MapGet("/", () => "Hello World!");

// Create
app.MapPost("/users", (User user) => {
    user.Id = users.Count > 0 ? users.Max(u => u.Id) + 1 : 1;
    users.Add(user);
    return Results.Created($"/users/{user.Id}", user);
});

// Read all
app.MapGet("/users", () => users);

// Read by ID
app.MapGet("/users/{id}", (int id) => {
    var user = users.FirstOrDefault(u => u.Id == id);
    return user is not null ? Results.Ok(user) : Results.NotFound();
});

// Update
app.MapPut("/users/{id}", (int id, User updatedUser) => {
    var user = users.FirstOrDefault(u => u.Id == id);
    if (user is null) return Results.NotFound();

    user.FullName = updatedUser.FullName;
    user.Age = updatedUser.Age;
    return Results.NoContent();
});

// Delete
app.MapDelete("/users/{id}", (int id) => {
    var user = users.FirstOrDefault(u => u.Id == id);
    if (user is null) return Results.NotFound();

    users.Remove(user);
    return Results.NoContent();
});

app.Run();

public record User
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public int Age { get; set; }
}
