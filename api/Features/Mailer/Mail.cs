using System.Net.Mail;
using Api.Features.Queue;

namespace Api.Features.Mailer;

public abstract class Mail: IQueuelable
{
    public abstract string Subject { get; set; }
    public abstract string To { get; set; }
    protected abstract string Handler();
    public virtual List<string>? Attachments { get; set; }

    public MailMessage ToMessage()
    {
        return new MailMessage
        {
            Subject = Subject,
            From = new MailAddress(GetRootSmtpEmail()),
            Body = Handler(),
            IsBodyHtml = true,
        };
    }

    private string GetRootSmtpEmail()
    {
        var config = new ConfigApp();
        return config.Get("smtp.email");
    }

    public async Task<EmailDispatchOutput> SendAsync()
    {
        IEmailDispatcher dispatcher = new EmailDispatcher();

        return await dispatcher.SendAsync(this);
    }

    public QueueOutput DispatchOnQueue()
    {
        return GetDispatcher().ExecuteOnQueue("signer.email", this);
    }

    private Dispatcher GetDispatcher()
    {
        return new Dispatcher();
    }

    public virtual string Serialize()
    {
        return Newtonsoft.Json.JsonConvert.SerializeObject(new
        {
            subject = Subject,
            to = To,
            body = Handler(),
            from = GetRootSmtpEmail()
        });
    }
}