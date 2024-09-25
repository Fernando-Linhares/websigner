namespace Api.Controllers.Requests;

public class SignFilePdfRequest
{
    public long CertId { get; set; }
    public IFormFile File { get; set; }
    public string Pin { get; set; }
}