using IMS.Application.Common.Interfaces;
using IMS.Infrastructure.Settings;
using Microsoft.Extensions.Options;
using MimeKit;
using MailKit.Security;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;

namespace IMS.Infrastructure.Services;
public class EmailService : IEmailService
{
    private readonly EmailSettings _emailSettings;

    public EmailService(IOptions<EmailSettings> emailSettings)
    {
        _emailSettings = emailSettings.Value;
    }

    public async Task SendEmailAsync(string email, string subject, string message)
    {
        var emailMessage = new MimeMessage();
        emailMessage.From.Add(new MailboxAddress(_emailSettings.SenderName, _emailSettings.SenderEmail));
        emailMessage.To.Add(new MailboxAddress("", email));
        emailMessage.Subject = subject;
        emailMessage.Body = new TextPart("html") { Text = message };

        using var client = new SmtpClient();
        await client.ConnectAsync(_emailSettings.SmtpServer, _emailSettings.Port, SecureSocketOptions.StartTls);
        await client.AuthenticateAsync(_emailSettings.SenderEmail, _emailSettings.Password);
        await client.SendAsync(emailMessage);
        await client.DisconnectAsync(true);
    }
}