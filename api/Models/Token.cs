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
        var config = new ConfigApp();
        var handler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(config.Get("app.key"));
        var exporation = config.Get("app.expiration.token");
        var expires = DateTime.UtcNow.AddHours(Convert.ToDouble(exporation));
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity((new Claim[]
            {
                new Claim(ClaimTypes.Sid, User.Id.ToString()),
                new Claim(ClaimTypes.Name, User.Name),
                new Claim(ClaimTypes.Email, User.Email),
            })),
            Expires = expires,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = handler.WriteToken(handler.CreateToken(tokenDescriptor));
        AccessToken = token;
        RefreshToken = AccessToken;
        Expires = expires;
    }
}