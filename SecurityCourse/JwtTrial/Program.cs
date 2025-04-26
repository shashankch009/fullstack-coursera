// See https://aka.ms/new-console-template for more information

var service = new JwtService();

string clientId = "some-client-id";
string userId = "user123";
string role = "admin";

string token = service.Create(clientId, userId, role);
Console.WriteLine(token);

service.Decode(token, clientId);
