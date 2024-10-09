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
    public class DisableUser
    {
        public class Command : IRequest<CommandResult<Result>>
        {
            public int Id { get; set; } // User ID to be disabled or enabled
            public bool IsActive { get; set; } // Flag to disable/enable user
        }

        public class Result
        {
            public string? Message { get; set; }
        }

        public class Handler(
            IRepository<User> userRepos,
            IAppLocalizer localizer,
            ILogger<DisableUser> logger,
            ILoggedUser loggedUser,
            IUnitOfWork unitOfWork
            ) : BaseRequestHandler<Command, CommandResult<Result>>(localizer, logger, loggedUser, unitOfWork)
        {
            public override async Task<CommandResult<Result>> Handle(Command request, CancellationToken cancellationToken)
            {
                // Ensure only admins can disable/enable users
                if (_loggedUser.RoleId != (int)ERole.Admin)
                {
                    return CommandResult<Result>.Fail(HttpStatusCode.Forbidden, _localizer["Only admin can disable or enable users"]);
                }

                // Find the user by ID
                var user = await userRepos.Queryable().FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
                if (user == null)
                {
                    return CommandResult<Result>.Fail(_localizer["User not found"]);
                }

                // Update the IsActive field
                user.IsActive = request.IsActive;

                // Save the changes
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return CommandResult<Result>.Success(new Result
                {
                    Message = _localizer[$"User {(request.IsActive ? "enabled" : "disabled")} successfully"]
                });
            }
        }
    }
}
