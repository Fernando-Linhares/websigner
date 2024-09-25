using System.Security.Claims;
using Api.Controllers.Requests;
using Api.Database;
using Api.Models;
using Api.Models.Enums;
using ModelFile=Api.Models.File;
using Api.Services.Signature;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Authorize]
[ApiController]
[Route("sign/pdf")]
public class SignPdfController : BaseController
{
   private readonly ISignature _signature;
   private readonly DataContext _context;
   
   public SignPdfController(ISignature signature, DataContext dataContext): base(dataContext)
   {
      _signature = signature;
      _context = dataContext;
   }
   
   [HttpPost]
   public async Task<IActionResult> SignPdf(SignFilePdfRequest request)
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
         var user = CurrentUser();
         var signature = await BeginSignature(user);

         var inputDto = new SignatureInput(
            cert,
            request.File,
            request.Pin);

         var output = await _signature.Signature(inputDto);
         var config = new ConfigApp();

         await DefireSignature(signature, SignatureStatus.Ok);
         await PersistFile(signature, output.SignedFile);

         await transaction.CommitAsync();
         return AnswerSuccess(new
         {
            url = config.Get("app.url") + "pdfs/" + output.SignedFile,
            timestamp = output.Timestamp
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

   private async Task PersistFile(SignatureProcess signature, string filename)
   {
      var file = new ModelFile
      {
         FileName = filename,
         SignatureProcessId = signature.Id,
      };
      _context.Files.Add(file);
      await _context.SaveChangesAsync();
   }
   
   private async Task DefireSignature(SignatureProcess signature, SignatureStatus status)
   {
      signature.Status = status;
      _context.SignatureProcesses.Update(signature);
      await _context.SaveChangesAsync();
   }

   private async Task<SignatureProcess> BeginSignature(User user)
   {
      var signature = new SignatureProcess
      {
         UserId = user.Id,
         Date = DateTime.Now
      };
      _context.SignatureProcesses.Add(signature);
      await _context.SaveChangesAsync();
      return signature;
   }
}