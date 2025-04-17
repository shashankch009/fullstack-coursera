using System.Text.Json;
using System.Xml.Serialization;
using System.Text.Json.Serialization;
var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "I am Root!");

var samplePerson = new Person { UserName = "John Doe", UserAge = 30 };

app.MapGet("/json-manual", () => {
    var json = JsonSerializer.Serialize(samplePerson); //case - as it is .
    return Results.Text(json, "application/json");
});

app.MapGet("/json-custom", () => {
    var options = new JsonSerializerOptions 
    {
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
        //smallcase separated with _
    };
    var json = JsonSerializer.Serialize(samplePerson, options);
    return Results.Text(json, "application/json");
});

app.MapGet("/json", () => {
    return Results.Json(samplePerson); //uses camelCase
});

app.MapGet("/auto", () => samplePerson); //camelCase 
//this does what above one does, but with less code
//it automatically serializes the object to JSON


app.MapGet("/xml", () => {
    var xmlSerializer = new XmlSerializer(typeof(Person));
    var stringWriter = new StringWriter();
    xmlSerializer.Serialize(stringWriter, samplePerson);
    var xmlOutput = stringWriter.ToString();
    return Results.Text(xmlOutput, "application/xml");
});

app.MapPost("/auto", (Person person) => {
    Console.WriteLine($"Received: {person.UserName}, {person.UserAge}");
    return Results.Json(person); //camelCase
});

app.MapPost("/json", async (HttpContext context) => {
    var person = await context.Request.ReadFromJsonAsync<Person>();
    Console.WriteLine($"Received: {person.UserName}, {person.UserAge}");
    return Results.Json(person); //camelCase
});

app.MapPost("/json-custom", async (HttpContext context) => {
    var options = new JsonSerializerOptions 
    {
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
        UnmappedMemberHandling = JsonUnmappedMemberHandling.Disallow
        //don't allow client to send extra members
    };
    var person = await context.Request.ReadFromJsonAsync<Person>(options);
    Console.WriteLine($"Received: {person.UserName}, {person.UserAge}");
    return Results.Json(person); //camelCase
});

app.MapPost("/json-manual", async (HttpContext context) => {
    using var reader = new StreamReader(context.Request.Body);
    var person = await JsonSerializer.DeserializeAsync<Person>(reader.BaseStream);
    Console.WriteLine($"Received: {person.UserName}, {person.UserAge}");
    return Results.Json(person); //camelCase
});

app.MapPost("/xml", async (HttpContext context) => {
    using var reader = new StreamReader(context.Request.Body);
    var body = await reader.ReadToEndAsync();

    var xmlSerializer = new XmlSerializer(typeof(Person));
    using var stringReader = new StringReader(body);
    var person = (Person)xmlSerializer.Deserialize(stringReader);
    Console.WriteLine($"Received: {person.UserName}, {person.UserAge}");
    return Results.Json(person); //camelCase
});

app.Run();

public class Person 
{
    public string UserName { get; set; }
    public int UserAge { get; set; }
}