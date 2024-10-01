namespace Api.Controllers.Crud.Requests;

public record CertificateRegisterRequest(string Name, IFormFile File);