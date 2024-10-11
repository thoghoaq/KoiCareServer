using FirebaseAdmin.Auth;
using KoiCare.Application.Abtractions.Authentication;

namespace KoiCare.Infrastructure.Dependencies.Firebase.Authentication
{
    public class AuthenticationService : IAuthenticationService
    {
        public async Task<string> RegisterAsync(string email, string password, CancellationToken cancellationToken)
        {
            var userRecord = await FirebaseAuth.DefaultInstance.CreateUserAsync(new UserRecordArgs
            {
                Email = email,
                Password = password
            }, cancellationToken);
            return userRecord.Uid;
        }

        public async Task DeleteAccountAsync(string uid, CancellationToken cancellationToken)
        {
            await FirebaseAuth.DefaultInstance.DeleteUserAsync(uid, cancellationToken);
        }

        public async Task<string> GeneratePasswordResetLinkAsync(string email)
        {
            return await FirebaseAuth.DefaultInstance.GeneratePasswordResetLinkAsync(email);
        }
    }
}
