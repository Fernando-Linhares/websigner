namespace Api.Controllers.Crud.Requests;

public class CertificateRegisterRequest
{
    public string Name { get; set; }
    public IFormFile file { get; set; }
}