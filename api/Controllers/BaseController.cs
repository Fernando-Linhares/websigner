using System.Security.Claims;
using Api.Database;
using Api.Models;
using Api.Models.Actions;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
public abstract class BaseController(DataContext context) : ControllerBase
{
    protected readonly DataContext _context = context;

    protected User? CurrentUser()
    {
        string? email =  User.FindFirst(ClaimTypes.Email)?.Value;
        
        User? user = _context.Users.FirstOrDefault(u => u.Email == email);
        
        return user;
    }

    protected IActionResult AnswerSuccess()
    {
        return Ok();
    }
    
    protected IActionResult AnswerSuccess(object result)
    {
        return Ok(new { data=result });
    }

    protected IActionResult AnswerPagination<T>(PaginationOutput<T> pagination)
    {
        return Ok(new
        {
            data = pagination.Data,
            page = pagination.Pagination,
        });
    }

    protected IActionResult AnswerInternalError()
    {
        return StatusCode(500, new { error = "internalError" });
    }
    
    protected IActionResult AnswerInternalError(string error)
    {
        return StatusCode(500, new { error = error });
    }

    protected IActionResult AnswerNotFound()
    {
        return NotFound();
    }

    protected IActionResult AnswerNotFound(object result)
    {
        return NotFound(result);
    }
}