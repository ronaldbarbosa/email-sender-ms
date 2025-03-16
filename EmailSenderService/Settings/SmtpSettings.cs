namespace EmailSenderService.Settings;

public class SmtpSettings
{
    public string From { get; init; } = string.Empty;
    public string To { get; init; }  = string.Empty;
    public string Server { get; init; } = string.Empty;
    public string Username { get; init; } = string.Empty;
    public string Password { get; init; }  = string.Empty;
    public int Port { get; init; }
}