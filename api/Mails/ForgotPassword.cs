using Api.Features.Mailer;
using Api.Models;

namespace Api.Mails;

public class ForgotPassword : Mail
{
    public override string Subject { get; set; } = "Forgot password - Reset Password";
    public override string To { get; set; }
    public User User { get; set; }
    public string Token { get; set; }

    protected override string Handler()
    {
        var config = new ConfigApp();
        string link = config.Get("app.frontend")  + "reset-password/" + Token;
        return $@"<h2> Hi {User.Name},</h2>
             <div>
                   We received a request to reset your password for your account.
                No worries, we’ve got you covered! Simply click the button below to create a
                new password and get back to accessing your account.
            <div>
            <div class='w-full flex justify-center py-6'>
                <a href='{link}' class='w-3/4 bg-sky-700 rounded p-1 text-slate-200 hover:bg-sky-800'>
                    Reset your password
                </a>
              </div>";
    }
}