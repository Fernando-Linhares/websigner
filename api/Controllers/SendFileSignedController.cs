using Api.Controllers.Requests;
using Api.Database;
using Api.Mails;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Authorize]
[ApiController]
[Route("/send/file")]
public class SendFileSignedController(DataContext context) : BaseController(context)
{
    [HttpPost]
    public async Task<IActionResult> SendFile([FromBody] SendPdfRequest request)
    {
        try
        {
            var file = await _context.Files.FindAsync(request.FileId);
            
            var mail = new SendPdf
            {
                By=CurrentUser().Name,
                To=request.Email,
                Attachments = new List<string>
                {
                    file.GetPath()
                }
            };
            
            return AnswerSuccess(await mail.SendAsync());
        }
        catch (Exception e)
        {
            return AnswerInternalError(e.Message);
        }
    }
}