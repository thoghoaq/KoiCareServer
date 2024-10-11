using KoiCare.Application.Abtractions.Authentication;
using KoiCare.Application.Abtractions.Configuration; // Thêm dòng này
using KoiCare.Application.Abtractions.Database;
using KoiCare.Application.Abtractions.Email;
using KoiCare.Application.Abtractions.Localization;
using KoiCare.Application.Commons;
using KoiCare.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options; // Thêm dòng này
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;

namespace KoiCare.Application.Features.Account
{
    public class ForgotPassword
    {
        public class Command : IRequest<CommandResult<Result>>
        {
            [EmailAddress]
            public required string Email { get; set; }
        }

        public class Result
        {
            public string? Message { get; set; }
        }
    }

    public class ForgotPasswordHandler : IRequestHandler<ForgotPassword.Command, CommandResult<ForgotPassword.Result>>
    {
        private readonly IRepository<User> _userRepos;
        private readonly IAuthenticationService _authenticationService;
        private readonly IEmailService _emailService;
        private readonly IAppLocalizer _localizer;
        private readonly ILogger<ForgotPasswordHandler> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly UrlsSettings _urlsSettings; // Thêm dòng này

        public ForgotPasswordHandler(
            IRepository<User> userRepos,
            IAuthenticationService authenticationService,
            IEmailService emailService,
            IAppLocalizer localizer,
            ILogger<ForgotPasswordHandler> logger,
            IUnitOfWork unitOfWork,
            IOptions<UrlsSettings> urlsSettings) // Thêm dòng này
        {
            _userRepos = userRepos;
            _authenticationService = authenticationService;
            _emailService = emailService;
            _localizer = localizer;
            _logger = logger;
            _unitOfWork = unitOfWork;
            _urlsSettings = urlsSettings.Value; // Thêm dòng này
        }

        public async Task<CommandResult<ForgotPassword.Result>> Handle(ForgotPassword.Command request, CancellationToken cancellationToken)
        {
            var user = await _userRepos.Queryable().FirstOrDefaultAsync(u => u.Email == request.Email, cancellationToken);

            if (user == null)
            {
                return CommandResult<ForgotPassword.Result>.Fail(_localizer["User not found"]);
            }

            try
            {
                // Tạo link reset mật khẩu sử dụng Firebase
                var resetLink = await _authenticationService.GeneratePasswordResetLinkAsync(user.Email);

                // Tạo URL reset mật khẩu sử dụng cấu hình từ appsettings.json
                var resetUrl = $"{_urlsSettings.FrontendBaseUrl}/reset-password?link={Uri.EscapeDataString(resetLink)}";

                // Gửi email chứa đường dẫn reset mật khẩu
                var emailResult = await _emailService.SendEmailAsync(user.Email, "Reset your password",
                    $"Please reset your password by clicking <a href='{resetUrl}'>here</a>.");

                if (!emailResult)
                {
                    return CommandResult<ForgotPassword.Result>.Fail(_localizer["Failed to send email"]);
                }

                return CommandResult<ForgotPassword.Result>.Success(new ForgotPassword.Result
                {
                    Message = _localizer["Password reset link has been sent to your email"]
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while handling ForgotPassword");
                return CommandResult<ForgotPassword.Result>.Fail(_localizer["An error occurred while processing your request"]);
            }
        }
    }
}
