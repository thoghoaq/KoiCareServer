using KoiCare.Application.Abtractions.Database;
using KoiCare.Application.Abtractions.Localization;
using KoiCare.Application.Abtractions.LoggedUser;
using KoiCare.Application.Commons;
using KoiCare.Domain.Entities;
using KoiCare.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Net;

namespace KoiCare.Application.Features.Account
{
    public class GetUser
    {
        public class Query : IRequest<CommandResult<Result>>
        {
            public int? Id { get; set; }
            public string? Email { get; set; }
            public string? PhoneNumber { get; set; }
        }

        public class Result
        {
            public int Id { get; set; }
            public string? Email { get; set; }
            public string? Username { get; set; }
            public int RoleId { get; set; }
            public string RoleName { get; set; } = string.Empty;
            public bool IsActive { get; set; }
            public bool IsAdmin => RoleId == (int)ERole.Admin;
            public DateTime? DateOfBirth { get; set; }
            public string? PhoneNumber { get; set; }
            public int? GenderId { get; set; }
        }

        public class Handler(
    IRepository<User> userRepos,
    IAppLocalizer localizer,
    ILogger<GetUser> logger,
    ILoggedUser loggedUser,
    IUnitOfWork unitOfWork
    ) : BaseRequestHandler<Query, CommandResult<Result>>(localizer, logger, loggedUser, unitOfWork)
        {
            private readonly IRepository<User> _userRepos = userRepos;

            public override async Task<CommandResult<Result>> Handle(Query request, CancellationToken cancellationToken)
            {
                try
                {
                    var email = request.Email ?? _loggedUser.Email;
                    var user = await _userRepos.Queryable()
                        .Include(u => u.Role)
                        .FirstOrDefaultAsync(x => request.Id.HasValue ? x.Id == request.Id : x.Email == email, cancellationToken);

                    if (user == null)
                    {
                        return CommandResult<Result>.Fail(_localizer["User not found"]);
                    }

                    if (_loggedUser.RoleId != (int)ERole.Admin && _loggedUser.Email != user.Email)
                    {
                        return CommandResult<Result>.Fail(HttpStatusCode.Forbidden, _localizer["Unauthorized"]);
                    }

                    return CommandResult<Result>.Success(new Result
                    {
                        Id = user.Id,
                        Email = user.Email,
                        Username = user.Username,
                        RoleId = user.RoleId,
                        RoleName = user.Role.Name,
                        IsActive = user.IsActive,
                        DateOfBirth = user.DateOfBirth,
                        PhoneNumber = user.PhoneNumber,
                        GenderId = user.GenderId
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