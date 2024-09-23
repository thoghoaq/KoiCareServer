using KoiCare.Application.Abtractions.Authentication;
using KoiCare.Application.Abtractions.Database;
using KoiCare.Application.Abtractions.Localization;
using KoiCare.Application.Commons;
using KoiCare.Domain.Entities;
using KoiCare.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace KoiCare.Application.Features.Account
{
    public class CreateUser
    {
        public class Command : IRequest<CommandResult<Result>>
        {
            [EmailAddress]
            public required string Email { get; set; }
            [MinLength(8)]
            public required string Password { get; set; }
            [MaxLength(255)]
            public required string Username { get; set; }
            public required ERole RoleId { get; set; }
        }

        public class Result
        {
            public string? Message { get; set; }
        }

        public class Handler(IAppLocalizer localizer, IRepository<User> userRepos, IUnitOfWork unitOfWork, IAuthenticationService authenticationService) : IRequestHandler<Command, CommandResult<Result>>
        {
            public async Task<CommandResult<Result>> Handle(Command request, CancellationToken cancellationToken)
            {
                try
                {
                    if (request.RoleId == ERole.Admin)
                    {
                        return CommandResult<Result>.Fail(localizer["Cannot create admin user"]);
                    }
                    var existedUser = await userRepos.Queryable().FirstOrDefaultAsync(x => x.Email == request.Email, cancellationToken);
                    if (existedUser != null)
                    {
                        return CommandResult<Result>.Fail(localizer["Email already exists"]);
                    }
                    var identityId = await authenticationService.RegisterAsync(request.Email, request.Password, cancellationToken);
                    if (string.IsNullOrEmpty(identityId))
                    {
                        return CommandResult<Result>.Fail(localizer["Firebase register return null"]);
                    }
                    var user = new User
                    {
                        Email = request.Email,
                        Username = request.Username,
                        RoleId = (int)request.RoleId,
                        IdentityId = identityId,
                        IsActive = true,
                    };
                    await userRepos.AddAsync(user, cancellationToken);
                    await unitOfWork.SaveChangesAsync(cancellationToken);
                    return CommandResult<Result>.Success(new Result
                    {
                        Message = localizer["User created successfully"],
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
