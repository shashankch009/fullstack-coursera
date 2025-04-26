
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace OAuthServer.Controllers;


[ApiController]
[Route("[controller]")]
public class OAuthController : ControllerBase
{
    private static readonly Dictionary<string, string> AuthCodes = new();

    [HttpGet("authorize")]
    public async Task<IActionResult> Authorize(string client_id, string redirect_uri, string response_type, string state)
    {
        // Validate the input parameters
        if (string.IsNullOrEmpty(client_id) || string.IsNullOrEmpty(redirect_uri) || string.IsNullOrEmpty(response_type))
        {
            return BadRequest("Missing required parameters.");
        }

        // Simulate authorization logic
        var authCode = Guid.NewGuid().ToString();

        AuthCodes[authCode] = client_id;

        // Redirect to the provided redirectUri with the authorization code
        var redirectUrl = $"{redirect_uri}?code={authCode}&state=xyz";
        return Redirect(redirectUrl);
    }

    [HttpPost("token")]
    public async Task<IActionResult> Token([FromForm] string auth_code, [FromForm] string client_id)
    {
        if(!AuthCodes.ContainsKey(auth_code) || AuthCodes[auth_code] != client_id) 
        {
            return BadRequest("Invalid authorization code or client ID");
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiaWF0IjoxNTE2MjM5MDIyfQ.MRvO3fE0o9C-pZfd3pI0hMDDXihJfQa1XPQ-UAelzaI"));
        //this above key strin is not supposed to be client secret. both are separate 
        //this one is completely managed by this server and is known to it only. 
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: "OAuthServer",
            audience: client_id,
            claims: new List<Claim> { new Claim("sub", "12345"), new Claim("name", "John Doe") },
            expires: DateTime.Now.AddMinutes(30),
            signingCredentials: creds
        );

        return Ok(new
        {
            access_token = new JwtSecurityTokenHandler().WriteToken(token),
            token_type = "Bearer",
            expires_in = 1800
        });
    }

}