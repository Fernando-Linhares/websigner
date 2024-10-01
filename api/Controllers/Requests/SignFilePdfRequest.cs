namespace Api.Controllers.Requests;

public record SignFilePdfRequest(long CertId, IFormFile File, string Pin);