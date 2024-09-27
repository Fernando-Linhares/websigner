using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Api.Database;

namespace Api.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class MeController : BaseController
{
    public MeController(DataContext context): base(context) {}

    [HttpGet]
    public async Task<IActionResult> GetUserIdentity()
    {
        var user = CurrentUser();
        
        return AnswerSuccess(new
        {
            Id = user?.Id,
            Name = user?.Name,
            Email = user?.Email
        });
    }
}