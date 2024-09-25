using Api.Models;

namespace Api.Services.Signature;

public record SignatureInput(Certificate Cert, IFormFile File,  string Pin);