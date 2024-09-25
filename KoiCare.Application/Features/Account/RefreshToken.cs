using KoiCare.Application.Abtractions.Authentication;
using KoiCare.Application.Abtractions.Database;
using KoiCare.Application.Abtractions.Localization;
using KoiCare.Application.Abtractions.LoggedUser;
using KoiCare.Application.Commons;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Net;

namespace KoiCare.Application.Features.Account
{
    public class RefreshToken
    {
        public class Command : IRequest<CommandResult<Result>>
        {
            public required string RefreshToken { get; set; }
        }

        public class Result
        {
            public string? AccessToken { get; set; }
            public string? RefreshToken { get; set; }
            public string? Message { get; set; }
        }

        public class Handler(
            IJwtProvider jwtProvider,
            IAppLocalizer localizer,
            ILogger<RefreshToken> logger,
            ILoggedUser loggedUser,
            IUnitOfWork unitOfWork
            ) : BaseRequestHandler<Command, CommandResult<Result>>(localizer, logger, loggedUser, unitOfWork)
        {
            private readonly IJwtProvider _jwtProvider = jwtProvider;

            public override async Task<CommandResult<Result>> Handle(Command request, CancellationToken cancellationToken)
            {
                try
                {
                    var response = await _jwtProvider.RefreshTokenAsync(request.RefreshToken, cancellationToken);
                    if (string.IsNullOrEmpty(response?.IdToken))
                    {
                        return CommandResult<Result>.Fail(_localizer["Invalid refresh token"]);
                    }

                    return CommandResult<Result>.Success(new Result
                    {
                        AccessToken = response!.IdToken,
                        RefreshToken = response.RefreshToken!,
                        Message = _localizer["Token refreshed"]
                    });
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, ex.Message);
                    return CommandResult<Result>.Fail(HttpStatusCode.InternalServerError, ex.Message);
                }
            }
        }
    }
}
