using EncryptionDemo.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IEncryptionService, EncryptionService>();

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.MapPost("/encrypt", (IEncryptionService es, EncryptRequest er) => {
    return Results.Ok(es.Encrypt(er.PlainText));
});

app.MapPost("/decrypt", (IEncryptionService es, DecryptRequest dr) => {
    return Results.Ok(es.Decrypt(dr.CipherText));
});


app.Run();

public record EncryptRequest(string PlainText);
public record DecryptRequest(string CipherText);