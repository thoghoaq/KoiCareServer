using KoiCare.Application.Abtractions.Authentication;
using KoiCare.Application.Abtractions.Database;
using KoiCare.Application.Abtractions.Email;
using KoiCare.Application.Abtractions.Localization;
using KoiCare.Application.Commons;
using KoiCare.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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

        public class Handler : IRequestHandler<Command, CommandResult<Result>>
        {
            private readonly IRepository<User> _userRepos;
            private readonly IAuthenticationService _authenticationService;
            private readonly IEmailService _emailService;
            private readonly IAppLocalizer _localizer;
            private readonly ILogger<ForgotPassword> _logger;
            private readonly IUnitOfWork _unitOfWork;

            public Handler(
                IRepository<User> userRepos,
                IAuthenticationService authenticationService,
                IEmailService emailService,
                IAppLocalizer localizer,
                ILogger<ForgotPassword> logger,
                IUnitOfWork unitOfWork)
            {
                _userRepos = userRepos;
                _authenticationService = authenticationService;
                _emailService = emailService;
                _localizer = localizer;
                _logger = logger;
                _unitOfWork = unitOfWork;
            }

            public async Task<CommandResult<Result>> Handle(Command request, CancellationToken cancellationToken)
            {
                var user = await _userRepos.Queryable().FirstOrDefaultAsync(u => u.Email == request.Email, cancellationToken);

                if (user == null)
                {
                    return CommandResult<Result>.Fail(_localizer["User not found"]);
                }

                try
                {
                    // Tạo link reset mật khẩu sử dụng Firebase
                    var resetLink = await _authenticationService.GeneratePasswordResetLinkAsync(user.Email);

                    // Tạo URL reset mật khẩu (có thể tùy chỉnh URL frontend của bạn)
                    var resetUrl = $"https://yourfrontend.com/reset-password?link={Uri.EscapeDataString(resetLink)}";

                    // Gửi email chứa đường dẫn reset mật khẩu
                    var emailResult = await _emailService.SendEmailAsync(user.Email, "Reset your password",
                        $"Please reset your password by clicking <a href='{resetUrl}'>here</a>.");

                    if (!emailResult)
                    {
                        return CommandResult<Result>.Fail(_localizer["Failed to send email"]);
                    }

                    return CommandResult<Result>.Success(new Result
                    {
                        Message = _localizer["Password reset link has been sent to your email"]
                    });
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error while handling ForgotPassword");
                    return CommandResult<Result>.Fail(_localizer["An error occurred while processing your request"]);
                }
            }
        }
    }
}
