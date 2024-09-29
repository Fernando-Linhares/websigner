using Api.Features.Mailer;

namespace Api.Mails;

public class ForgotPassword : Mail
{
    public override string Subject { get; set; } = "Forgot password - Reset Password";
    public override string To { get; set; }

    protected override string Handler()
    {
        var config = new ConfigApp();
        string link = config.Get("app.frontend")  + "reset-password";
        return  $"Reset your password <div><b>Click right here</b> <a href=\"{link}\">reset password</a> </div>";
    }
}