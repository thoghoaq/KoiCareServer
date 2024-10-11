using System.Threading.Tasks;

namespace KoiCare.Application.Abtractions.Email
{
    public interface IEmailService
    {
        Task<bool> SendEmailAsync(string email, string subject, string htmlMessage);
    }
}