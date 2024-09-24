using KoiCare.Application.Abtractions.Authentication;
using KoiCare.Application.Abtractions.Localization;
using KoiCare.Application.Commons;
using MediatR;
using System.Net;

namespace KoiCare.Application.Features.Account
{
    public class LoginUser
    {
        public class Command : IRequest<CommandResult<Result>>
        {
            public required string Email { get; set; }
            public required string Password { get; set; }
        }

        // cde ádasd

        public class Result
        {
            public string? AccessToken { get; set; }
            public string? RefreshToken { get; set; }
            public string? Message { get; set; }
        }

        public class Handler(IJwtProvider jwtProvider, IAppLocalizer localizer) : IRequestHandler<Command, CommandResult<Result>>
        {
            public async Task<CommandResult<Result>> Handle(Command request, CancellationToken cancellationToken)
            {
                try
                {
                    var response = await jwtProvider.GenerateTokenAsync(request.Email, request.Password, cancellationToken);
                    if (string.IsNullOrEmpty(response?.IdToken))
                    {
                        return CommandResult<Result>.Fail(localizer["Wrong email or password"]);
                    }

                    return CommandResult<Result>.Success(new Result
                    {
                        AccessToken = response!.IdToken,
                        RefreshToken = response.RefreshToken!,
                    });
                }
                catch (Exception ex)
                {
                    return CommandResult<Result>.Fail(HttpStatusCode.InternalServerError, ex.Message);
                }
            }
        }
    }
}
