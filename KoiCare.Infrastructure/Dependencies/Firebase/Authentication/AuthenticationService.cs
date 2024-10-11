using FirebaseAdmin.Auth;
using KoiCare.Application.Abtractions.Authentication;
using System.Threading;
using System.Threading.Tasks;

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
            // Gửi link reset mật khẩu đến email người dùng
            return await FirebaseAuth.DefaultInstance.GeneratePasswordResetLinkAsync(email);
        }
    }
}
