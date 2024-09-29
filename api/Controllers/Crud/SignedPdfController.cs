using Api.Database;
using Api.Models;
using Api.Models.Actions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ModelFile = Api.Models.File;

namespace Api.Controllers.Crud;

[Authorize]
[ApiController]
[Route("/files")]
public class SignedPdfController : BaseController
{
   public SignedPdfController(DataContext context) : base(context) {}

   [HttpGet]
   public async Task<IActionResult> Index(
      [FromQuery(Name = "page")] int page = 1,
      [FromQuery(Name = "skipPagination")] bool skipPagination = false,
      [FromQuery(Name = "perPage")] int perPage = 20)
   {
      var query = _context.Files
         .Where(f => f.DeletedAt == null && f.UserId == CurrentUser().Id)
         .OrderByDescending(f => f.CreatedAt);
      
      if (skipPagination)
      {
         return AnswerSuccess(new
         {
            data = await query.ToListAsync()
         });
      }
      var totalCount = await query.CountAsync(f => f.DeletedAt == null && f.UserId == CurrentUser().Id);

      var input = new PaginationInput<ModelFile>(page, perPage, totalCount, query);
      var model = new ModelFile();
      return AnswerPagination(await model.Paginate(input));
   }
}