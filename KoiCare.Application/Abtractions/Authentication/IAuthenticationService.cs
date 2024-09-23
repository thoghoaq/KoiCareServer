namespace KoiCare.Application.Abtractions.Authentication
{
    public interface IAuthenticationService
    {
        Task<string> RegisterAsync(string email, string password, CancellationToken cancellationToken);
        Task DeleteAccountAsync(string uid, CancellationToken cancellationToken);
        Task<string> GeneratePasswordResetLinkAsync(string email);
    }
}
