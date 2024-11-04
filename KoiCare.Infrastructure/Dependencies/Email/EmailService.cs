using KoiCare.Application.Abtractions.Email;
using KoiCare.Domain.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net.Mail;

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

        public async Task SendEmailAsync(string toEmails, string subject, EEmailTemplate template, Dictionary<string, object> parameters)
        {
            try
            {
                string fromEmail = _smtpSettings.FromEmail;

                var body = await ReadTemplateFileAsync(template, parameters);
                var message = new MailMessage()
                {
                    From = new MailAddress(fromEmail),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true,
                };
                foreach (var email in toEmails.Split(","))
                {
                    message.To.Add(email);
                }

                _smtpClient.Send(message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending email");
            }
        }

        private static async Task<string> ReadTemplateFileAsync(EEmailTemplate template, Dictionary<string, object> parameters)
        {
            var fileName = template switch
            {
                EEmailTemplate.CustomerOrder => "OrderConfirmationTemplate.html",
                EEmailTemplate.PasswordReset => "PasswordResetTemplate.html",
                _ => throw new ArgumentOutOfRangeException(nameof(template), template, null)
            };

            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Dependencies\Email\Templates", fileName);

            var content = await File.ReadAllTextAsync(path);
            return BuildEmailContent(content, parameters);
        }

        private static string BuildEmailContent(string content, Dictionary<string, object> parameters)
        {
            foreach (var param in parameters)
            {
                content = content.Replace($"{{{{{param.Key}}}}}", param.Value.ToString());
            }
            return content;
        }
    }
}