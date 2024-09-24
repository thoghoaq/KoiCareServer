using KoiCare.Application.Abtractions.Authentication;
using KoiCare.Application.Abtractions.Database;
using KoiCare.Application.Abtractions.Localization;
using KoiCare.Application.Commons;
using KoiCare.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
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

        public class Result
        {
            public string? AccessToken { get; set; }
            public string? RefreshToken { get; set; }
            public int RoleId { get; set; }
            public string RoleName { get; set; } = string.Empty;
            public string UserName { get; set; } = string.Empty;
            public string? Message { get; set; }
        }

        public class Handler(IJwtProvider jwtProvider, IAppLocalizer localizer, IRepository<User> userRepos) : IRequestHandler<Command, CommandResult<Result>>
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

                    var user = await userRepos.Queryable()
                        .Include(u => u.Role)
                        .FirstOrDefaultAsync(x => x.Email == request.Email, cancellationToken);
                    if (user == null)
                    {
                        return CommandResult<Result>.Fail(localizer["User not found"]);
                    }

                    return CommandResult<Result>.Success(new Result
                    {
                        AccessToken = response!.IdToken,
                        RefreshToken = response.RefreshToken!,
                        RoleId = user.RoleId,
                        RoleName = user.Role.Name,
                        UserName = user.Username,
                        Message = localizer["Login successfully"]
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
