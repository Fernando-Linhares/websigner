using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Api.Database;
using Api.Models;

namespace Api.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class AuthController: BaseController
{
    private readonly DataContext _context;
    
    public AuthController(DataContext context): base(context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<IActionResult> Login([FromBody] Requests.LoginRequest request)
    {
        User? user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
        if (user is null)
        {
            return Unauthorized();
        }

        if (BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
        {
            var token = new Token
            {
                User = user
            };
            token.GenerateToken();
            _context.Tokens.Add(token);
            await _context.SaveChangesAsync();
            
            return  AnswerSuccess(new
            {
                Id = token.Id,
                Token = token.AccessToken,
                Expires = ToTimestamp(token.Expires),
            });
        }

        return Unauthorized();
    }

    private long ToTimestamp(DateTime expires)
    {
        DateTimeOffset dateTimeOffset = new DateTimeOffset(expires);
        return dateTimeOffset.ToUnixTimeSeconds();
    }
    
    [HttpPost]
    public async Task<IActionResult> Register([FromBody] Requests.RegisterRequest request)
    {
        var user = new User
        {
            Name = request.Name,
            Email = request.Email,
            Password = BCrypt.Net.BCrypt.HashPassword(request.Password),
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now
        };
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        var token = new Token
        {
            User = user
        };
        token.GenerateToken();
        _context.Tokens.Add(token);
        await _context.SaveChangesAsync();

        return AnswerSuccess(new
        {
            Id = token.Id,
            Token = token.AccessToken,
            Expires = ToTimestamp(token.Expires),
            Now = ToTimestamp(DateTime.Now)
        });
    }
}