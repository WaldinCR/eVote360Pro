using eVote360Pro.Core.Application.Dtos.Email;
using eVote360Pro.Core.Domain.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using eVote360Pro.Core.Application.Interfaces;

namespace eVote360Pro.Infrastructure.Shared.Services
{
    public class EmailService : IEmailService
    {
        private readonly MailSettings _mailSettings;
        private readonly ILogger<EmailService> _logger;
        public EmailService(IOptions<MailSettings> mailSettings, ILogger<EmailService> logger)
        {
            _mailSettings = mailSettings.Value;
            _logger = logger;
        }
        
        public async Task<bool> SendAsync(EmailRequestDto emailRequestDto)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(emailRequestDto.To))
                    emailRequestDto.ToRange?.Add(emailRequestDto.To);

                MimeMessage email = new()
                {
                    Sender = MailboxAddress.Parse(_mailSettings.EmailFrom),
                    Subject = emailRequestDto.Subject
                };

                foreach (var toItem in emailRequestDto.ToRange ?? [])
                    email.To.Add(MailboxAddress.Parse(toItem));

                BodyBuilder builder = new() { HtmlBody = emailRequestDto.HtmlBody };
                email.Body = builder.ToMessageBody();

                using MailKit.Net.Smtp.SmtpClient smtpClient = new();
                await smtpClient.ConnectAsync(_mailSettings.SmtpHost, _mailSettings.SmtpPort, MailKit.Security.SecureSocketOptions.StartTls);
                await smtpClient.AuthenticateAsync(_mailSettings.SmtpUser, _mailSettings.SmtpPass);
                await smtpClient.SendAsync(email);
                await smtpClient.DisconnectAsync(true);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error enviando correo: {Exception}", ex.Message);
                return false;
            }
        }

    }


}



