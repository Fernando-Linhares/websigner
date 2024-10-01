using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Api.Models;

public class Token
{
    [Key]
    public long Id { get; set; }
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
    public DateTime Expires { get; set; }
    public User? User { get; set; }
    public long UserId { get; set; }

    public void GenerateToken()
    {
        var expires = DateTime.UtcNow.AddHours(Convert.ToDouble(GetDefaultExpirationDate()));
        var key = GetKey();

        AccessToken = JwtToken(expires, key);
        RefreshToken = AccessToken;
        Expires = expires;
    }

    private string JwtToken(DateTime expiration, byte[] key)
    {
        var handler = new JwtSecurityTokenHandler();
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity((new Claim[]
            {
                new Claim(ClaimTypes.Sid, User.Id.ToString()),
                new Claim(ClaimTypes.Name, User.Name),
                new Claim(ClaimTypes.Email, User.Email),
            })),
            Expires = expiration,
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
        };
        return handler.WriteToken(handler.CreateToken(tokenDescriptor));
    }

    public void GenerateVisitToken()
    {
        var key = GetKey();
        var expires = DateTime.UtcNow.AddMinutes(1);
        AccessToken = JwtToken(expires, key);
        RefreshToken = AccessToken;
    }

    private string? GetDefaultExpirationDate()
    {
        var config = new ConfigApp();
        return config.Get("app.expiration.token");
    }

    private byte[] GetKey()
    {
        var config = new ConfigApp();
        return Encoding.ASCII.GetBytes(config.Get("app.key"));
    }
}