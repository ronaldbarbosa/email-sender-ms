namespace EmailSenderService.DTOs.Request;

public record EmailRequestDto(string Name, string From, string Subject, string Body);