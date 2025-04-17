
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

public class SecureUser 
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }

    public void Encrypt()
    {
        Password = Convert.ToBase64String(Encoding.GetEncoding("UTF-8").GetBytes(Password));
    }

    public string GenerateHash()
    {
        using (var sha256 = SHA256.Create())
        {
            byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(ToString()));
            return Convert.ToBase64String(hashBytes);
        }
    }

    public string Serialize()
    {
        if(string.IsNullOrEmpty(Password) || string.IsNullOrEmpty(Name) || string.IsNullOrEmpty(Email))
        {
            //input validation for serialization
            Console.WriteLine("Some fields are empty, cannot serialize.");
            return string.Empty;
        }

        Encrypt();
        return JsonSerializer.Serialize(this);
    }

    public static string Serialize(SecureUser user)
    {
        if(user == null) return string.Empty;
        return user.Serialize();
    }

    public static SecureUser? Deserialize(string json, bool secureSource = false)
    {
        if(!secureSource) 
        {
            Console.WriteLine("Deserialization blocked: Untrusted source.");
            return null;
        }

        var user = JsonSerializer.Deserialize<SecureUser>(json);
        if(user == null) return null;
        user.Password = Encoding.UTF8.GetString(Convert.FromBase64String(user.Password));
        return user;
    }

    public override string ToString() => JsonSerializer.Serialize(this);


}