using EmailSenderService.DTOs.Request;
using EmailSenderService.Settings;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace EmailSenderService.Services;

public class EmailService(IOptions<SmtpSettings> smtpSettings)
{
    private readonly SmtpSettings _smtpSettings = smtpSettings.Value;

    public async Task<bool> SendEmail(EmailRequestDto emailRequest)
    {
        try
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Contato Site", _smtpSettings.From));
            message.To.Add(new MailboxAddress("", _smtpSettings.To));
            message.Subject = emailRequest.Subject;
            var emailBody = FormatMessageBody(emailRequest);
            message.Body = new TextPart("html") { Text = emailBody };
            
            using var client = new SmtpClient();
            
            await client.ConnectAsync(_smtpSettings.Server, _smtpSettings.SmtpPort, SecureSocketOptions.StartTls);
            await client.AuthenticateAsync(_smtpSettings.Username, _smtpSettings.Password);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
            return true;

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    private static string FormatMessageBody(EmailRequestDto emailRequest)
    {
        return $@"
        <html>
        <head>
            <style>
                body {{
                    font-family: Arial, sans-serif;
                    line-height: 1.6;
                }}
                .container {{
                    max-width: 600px;
                    margin: 0 auto;
                    padding: 20px;
                    border: 1px solid #ddd;
                    border-radius: 8px;
                    background-color: #f9f9f9;
                }}
                .header {{
                    background-color: #007bff;
                    color: #ffffff;
                    padding: 10px;
                    text-align: center;
                    font-size: 20px;
                    border-radius: 8px 8px 0 0;
                }}
                .content {{
                    padding: 20px;
                    background: #fff;
                    border-radius: 0 0 8px 8px;
                }}
                .footer {{
                    margin-top: 20px;
                    text-align: center;
                    font-size: 12px;
                    color: #666;
                }}
            </style>
        </head>
        <body>
            <div class='container'>
                <div class='header'>Nova Mensagem Recebida</div>
                <div class='content'>
                    <p><strong>Nome:</strong> { emailRequest.Name }</p>
                    <p><strong>Email:</strong> { emailRequest.From }</p>
                    <p><strong>Assunto:</strong> { emailRequest.Subject }</p>
                    <p><strong>Mensagem:</strong></p>
                    <p>{ emailRequest.Body }</p>
                </div>
                <div class='footer'>Este e-mail foi enviado automaticamente.</div>
            </div>
        </body>
        </html>";
    }
}