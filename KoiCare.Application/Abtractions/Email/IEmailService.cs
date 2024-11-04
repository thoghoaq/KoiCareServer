using KoiCare.Domain.Enums;

namespace KoiCare.Application.Abtractions.Email
{
    public interface IEmailService
    {
        Task SendEmailAsync(string toEmails, string subject, EEmailTemplate template, Dictionary<string, object> parameters);
    }
}