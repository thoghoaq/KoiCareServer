using System.Net.Mail;
using System.Threading.Tasks;
using KoiCare.Application.Abtractions.Email;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;

namespace KoiCare.Infrastructure.Services
{
    public class EmailService : IEmailService
    {
        private readonly SmtpClient _smtpClient;
        private readonly SmtpSettings _smtpSettings;
        private readonly ILogger<EmailService> _logger;

        public EmailService(SmtpClient smtpClient, IOptions<SmtpSettings> smtpSettings, ILogger<EmailService> logger)
        {
            _smtpClient = smtpClient;
            _smtpSettings = smtpSettings.Value;
            _logger = logger;
        }

        public async Task<bool> SendEmailAsync(string email, string subject, string htmlMessage)
        {
            try
            {
                var mailMessage = new MailMessage
                {
                    From = new MailAddress(_smtpSettings.FromEmail, _smtpSettings.FromName),
                    Subject = subject,
                    Body = htmlMessage,
                    IsBodyHtml = true,
                };
                mailMessage.To.Add(email);

                await _smtpClient.SendMailAsync(mailMessage);
                return true;
            }
            catch (Exception ex)
            {
                // Ghi log lỗi
                _logger.LogError(ex, $"Failed to send email to {email} with subject {subject}.");
                return false;
            }
        }
    }
}