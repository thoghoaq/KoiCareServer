using KoiCare.Domain.Dtos;

namespace KoiCare.Application.Abtractions.Authentication
{
    public interface IJwtProvider
    {
        Task<AuthToken?> GenerateTokenAsync(string email, string password, CancellationToken cancellationToken);
        Task<AuthRefreshToken?> RefreshTokenAsync(string refreshToken, CancellationToken cancellationToken);
    }
}
