using Api.Features.Mailer;

namespace Api.Mails;

public class SendPdf: Mail
{
    public override string Subject { get; set; } = "Sending PDF";
    public override string To { get; set; }
    public override List<string>? Attachments { get; set; }
    public string By;
    protected override string Handler()
    {
        return $"Hi its a pdf file signed!<div style=\"color: rgba(0,0,0,0.3); margin-left:4px;\">Signed By {By}</div>";
    }
}