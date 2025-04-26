using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

public class JwtService 
{
    private const string SecretKey = "MySuperSecretKeyForThisDemoApp123456789"; // Key should be stored securely
    private const string Issuer = "JwtTrialApp";

    public string Create(string clientId, string userId, string role)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new []
        {
            new Claim(JwtRegisteredClaimNames.Sub, userId),
            new Claim("role", role),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var token = new JwtSecurityToken(
            issuer : Issuer,
            audience : clientId,
            claims : claims,
            expires : DateTime.Now.AddMinutes(10),
            signingCredentials : credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public void Decode(string token, string clientId)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey));
        var tokenHandler = new JwtSecurityTokenHandler();
        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = Issuer,
            ValidAudience = clientId,
            IssuerSigningKey = securityKey
        };

        try
        {
            var principal = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);
            Console.WriteLine("Decoded Claims:");
            foreach (var claim in principal.Claims)
            {
                Console.WriteLine($"{claim.Type}: {claim.Value}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Token validation failed: {ex.Message}");
        }
    }
}