namespace Api.Controllers.Requests;

public record SendPdfRequest(string Email, long FileId);