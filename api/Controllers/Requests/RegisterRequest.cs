namespace Api.Controllers.Requests;

public record RegisterRequest(string Name, string Email, string Password);