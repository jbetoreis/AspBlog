using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AspBlog.Models;
using Microsoft.IdentityModel.Tokens;

namespace AspBlog.Services;

public class TokenService
{
    public string GenerateToken(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(Configuration.JwtKey);
        var tokenDescription = new SecurityTokenDescriptor
        {
            Subject  = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, "Lorem"),
                new Claim(ClaimTypes.Role, "admin")
            }),
            Expires = DateTime.UtcNow.AddHours(2),
            SigningCredentials =
                new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescription);
        return tokenHandler.WriteToken(token);
    }
}