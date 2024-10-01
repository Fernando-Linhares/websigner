using Api.Controllers.Requests;
using Api.Database;
using Api.Services.Signature;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Authorize]
[ApiController]
[Route("sign/pdf")]
public class SignPdfController(ISignature signature, DataContext dataContext) : BaseController(dataContext)
{
   private readonly DataContext _context = dataContext;

   [HttpPost]
   public async Task<IActionResult> SignPdf([FromForm] SignFilePdfRequest request)
   {
      var cert = await _context.Certificates.FindAsync(request.CertId);

      if (cert == null)
      {
         return NotFound(new 
         {
            notFound = new
            {
               id = request.CertId
            }
         });
      }

      await using var transaction = _context.Database.BeginTransaction();

      try
      {
         var inputDto = new SignatureInput(
            cert,
            request.File,
            request.Pin);

         var output = await signature.Signature(inputDto);
         var fileSinged = new Api.Models.File
         {
            FileName = output.SignedFile,
            UserId = CurrentUser().Id,
            CreatedAt = DateTime.Now
         };
         _context.Files.Add(fileSinged);
         await _context.SaveChangesAsync();
         
         await transaction.CommitAsync();
         return AnswerSuccess(new
         {
            url = fileSinged.Url(),
            timestamp = fileSinged.CreatedAtTimestamp()
         });
      }
      catch (Exception e)
      {
         await transaction.RollbackAsync();
         
         return StatusCode(500, new
         {
            internalError=new
            {
               error = e.Message,
               inner = e.InnerException?.Message
            }
         });
      }
   }
}