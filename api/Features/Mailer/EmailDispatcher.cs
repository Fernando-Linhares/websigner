using System.Net;
using System.Net.Mail;

namespace Api.Features.Mailer;

public class EmailDispatcher: IEmailDispatcher
{
    private SmtpClient _smtpClient;
    
    public EmailDispatcher()
    {
        var config = new ConfigApp();
        _smtpClient = new SmtpClient(config.Get("smtp.host"), Int32.Parse(config.Get("smtp.port")))
        {
            Credentials = new NetworkCredential( config.Get("smtp.username"), config.Get("smtp.password")),
            EnableSsl = false
        };
    }
    
    public async Task<EmailDispatchOutput> SendAsync(Mail mail)
    {
        var instance = mail.ToMessage();
        instance.To.Add(mail.To);
        if (mail.Attachments != null)
        {
            foreach (string path in mail.Attachments)
            {
                instance.Attachments.Add(new Attachment(path));
            }
        }
        await _smtpClient.SendMailAsync(instance);

        return new EmailDispatchOutput(mail.To, mail.Subject, GetTimestamp());
    }

    private long GetTimestamp()
    {
        return DateTimeOffset.UtcNow.ToUnixTimeSeconds();
    }
}