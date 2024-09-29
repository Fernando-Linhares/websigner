namespace Api.Controllers.Requests;

public class SendPdfRequest
{
    public string Email { get; set; }
    public long FileId { get; set; }
}