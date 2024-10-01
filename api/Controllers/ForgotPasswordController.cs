using Api.Database;
using Microsoft.AspNetCore.Mvc;
using Api.Controllers.Requests;
using System;
using System.Net;
using System.Net.Mail;
using Api.Mails;
using Api.Models;

namespace Api.Controllers;

[ApiController]
[Route("/forgot-password")]
public class ForgotPasswordController(DataContext context) : BaseController(context)
{
    [HttpPost]
    public  async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request)
    {
        try
        {
            User? user = _context.Users.FirstOrDefault(u => u.Email == request.Email);

            var token = new Token
            {
                User = user,
            };
            token.GenerateVisitToken();
            _context.Tokens.Add(token);
            
            await _context.SaveChangesAsync();
            
            var mail = new ForgotPassword();
            mail.To = request.Email;
            mail.Token = token.AccessToken;
            mail.User = user;
            
            var queueOutput = mail.DispatchOnQueue();
            return AnswerSuccess(queueOutput);
        }
        catch (Exception e)
        {
           return AnswerInternalError(e.Message);
        }
    }
}