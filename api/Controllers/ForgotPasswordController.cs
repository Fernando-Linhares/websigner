using Api.Database;
using Microsoft.AspNetCore.Mvc;
using Api.Controllers.Requests;
using System;
using System.Net;
using System.Net.Mail;
using Api.Mails;

namespace Api.Controllers;

[ApiController]
[Route("/forgot-password")]
public class ForgotPasswordController : BaseController
{
    public ForgotPasswordController(DataContext context) : base(context) {}

    [HttpPost]
    public  IActionResult ForgotPassword([FromBody] ForgotPasswordRequest request)
    {
        try
        {
            var mail = new ForgotPassword();
            mail.To = request.Email;
            var queueOutput = mail.DispatchOnQueue();
            return AnswerSuccess(queueOutput);
        }
        catch (Exception e)
        {
           return AnswerInternalError(e.Message);
        }
    }
}