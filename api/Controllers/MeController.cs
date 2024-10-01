using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Api.Database;

namespace Api.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class MeController(DataContext context) : BaseController(context)
{
    [HttpGet]
    public Task<IActionResult> GetUserIdentity()
    {
        var user = CurrentUser();
        
        return Task.FromResult(AnswerSuccess(new
        {
            Id = user?.Id,
            Name = user?.Name,
            Email = user?.Email
        }));
    }
}