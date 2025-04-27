
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

public class JwtService 
{
    private readonly IConfiguration configuration;
    public JwtService(IConfiguration configuration)
    {
        this.configuration = configuration;
    }

    public string Generate(string userName, string role)
    {
        var secretKey = configuration["Authorization:JwtSecurityKey"];

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        var claims = new []
        {
            new Claim(JwtRegisteredClaimNames.Sub, userName),
            new Claim("role", role),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var issuer = configuration["Authorization:JwtIssuer"];
        var audience = configuration["Authorization:JwtAudience"];

        var token = new JwtSecurityToken(
            issuer : issuer, 
            audience : audience, 
            claims : claims, 
            expires : DateTime.UtcNow.AddMinutes(30),
            signingCredentials : credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}