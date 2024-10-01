using Api.Controllers.Requests;
using Api.Database;
using Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Authorize]
[ApiController]
[Route("/reset-password")]
public class ResetPasswordController(DataContext context): BaseController(context)
{
    [HttpPost]
    public async Task<IActionResult> Reset([FromBody] ResetPasswordRequest request)
    {
        User? user = CurrentUser();

        if (user == null)
        {
            return BadRequest();
        }
        
        user.Password =  BCrypt.Net.BCrypt.HashPassword(request.Password);
        _context.Users.Update(user);
       await _context.SaveChangesAsync();

        return AnswerSuccess(new
        {
            user.Id,
            user.Email
        });
    }
}