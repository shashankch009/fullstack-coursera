using System.Text.Json;
using System.Xml.Serialization;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "I am Root!");

var samplePerson = new Person { UserName = "John Doe", UserAge = 30 };

app.MapGet("/manual-json", () => {
    var json = JsonSerializer.Serialize(samplePerson); //case - as it is .
    return Results.Text(json, "application/json");
});

app.MapGet("/custom-json", () => {
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

app.Run();

public class Person 
{
    public string UserName { get; set; }
    public int UserAge { get; set; }
}