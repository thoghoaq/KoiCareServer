using KoiCare.Application.Abtractions.Database;
using KoiCare.Application.Abtractions.Localization;
using KoiCare.Application.Abtractions.LoggedUser;
using KoiCare.Application.Commons;
using KoiCare.Domain.Entities;
using KoiCare.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace KoiCare.Application.Features.Account
{
    public class UpdateUser
    {
        public class Command : IRequest<CommandResult<Result>>
        {
            public int Id { get; set; }
            public required string Username { get; set; }
            public ERole? RoleId { get; set; } // Allow nullable for RoleId (default set to User role)

            public DateTime? DateOfBirth { get; set; }
            public string? PhoneNumber { get; set; }
            public EGender? Gender { get; set; }
        }


        public class Result
        {
            public string? Message { get; set; }
        }

        public class Handler(
    IRepository<User> userRepos,
    IAppLocalizer localizer,
    ILogger<UpdateUser> logger,
    ILoggedUser loggedUser,
    IUnitOfWork unitOfWork
    ) : BaseRequestHandler<Command, CommandResult<Result>>(localizer, logger, loggedUser, unitOfWork)
        {
            public override async Task<CommandResult<Result>> Handle(Command request, CancellationToken cancellationToken)
            {
                var currentUserId = _loggedUser.UserId;
                var currentUserRole = _loggedUser.RoleId;

                // Fetch the user to be updated
                var user = await userRepos.Queryable().FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
                if (user == null)
                {
                    return CommandResult<Result>.Fail(_localizer["User not found"]);
                }

                // Check if the logged-in user is trying to update someone else's info
                if (currentUserRole != (int)ERole.Admin && currentUserId != request.Id)
                {
                    return CommandResult<Result>.Fail(_localizer["You can only update your own account"]);
                }

                // Only admin can update RoleId, default to 'User' role if not admin
                if (currentUserRole != (int)ERole.Admin)
                {
                    user.RoleId = (int)ERole.User; // Default roleId to 'User' (2)
                }
                else if (request.RoleId.HasValue)
                {
                    user.RoleId = (int)request.RoleId.Value; // Admin can set RoleId
                }

                user.Username = request.Username;
                user.DateOfBirth = request.DateOfBirth.HasValue
                    ? DateTime.SpecifyKind(request.DateOfBirth.Value, DateTimeKind.Utc)
                    : (DateTime?)null;

                user.PhoneNumber = request.PhoneNumber;

                if (request.Gender.HasValue)
                {
                    user.GenderId = (int)request.Gender.Value; 
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return CommandResult<Result>.Success(new Result
                {
                    Message = _localizer["User updated successfully"],
                });
            }
        }

    }
}
