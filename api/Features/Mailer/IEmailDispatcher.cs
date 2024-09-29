namespace Api.Features.Mailer;

public interface IEmailDispatcher
{
    public Task<EmailDispatchOutput> SendAsync(Mail mail);
}