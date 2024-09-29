namespace Api.Features.Mailer;

public record EmailDispatchOutput(string To, string Subject, long Time);