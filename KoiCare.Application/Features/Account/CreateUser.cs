using KoiCare.Application.Abtractions.Authentication;
using KoiCare.Application.Abtractions.Database;
using KoiCare.Application.Abtractions.Localization;
using KoiCare.Application.Abtractions.LoggedUser;
using KoiCare.Application.Commons;
using KoiCare.Domain.Entities;
using KoiCare.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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

            [MinLength(8)]
            [Compare("Password", ErrorMessage = "Password and Confirm Password do not match.")]
            public required string ConfirmPassword { get; set; }

            [MaxLength(255)]
            public required string Username { get; set; }
            public ERole RoleId { get; set; } = ERole.User; // Default to User
        }



        public class Result
        {
            public string? Message { get; set; }
        }

        public class Handler(
            IRepository<User> userRepos,
            IAuthenticationService authenticationService,
            IAppLocalizer localizer,
            ILogger<CreateUser> logger,
            ILoggedUser loggedUser,
            IUnitOfWork unitOfWork
            ) : BaseRequestHandler<Command, CommandResult<Result>>(localizer, logger, loggedUser, unitOfWork)
        {
            private readonly IAuthenticationService _authenticationService = authenticationService;
            private readonly IRepository<User> _userRepos = userRepos;

            public override async Task<CommandResult<Result>> Handle(Command request, CancellationToken cancellationToken)
            {
                try
                {
                    if (request.Password != request.ConfirmPassword)
                    {
                        return CommandResult<Result>.Fail(_localizer["Password and Confirm Password do not match"]);
                    }

                    var existedUser = await _userRepos.Queryable().FirstOrDefaultAsync(x => x.Email == request.Email, cancellationToken);
                    if (existedUser != null)
                    {
                        return CommandResult<Result>.Fail(_localizer["Email already exists"]);
                    }

                    var identityId = await _authenticationService.RegisterAsync(request.Email, request.Password, cancellationToken);
                    if (string.IsNullOrEmpty(identityId))
                    {
                        return CommandResult<Result>.Fail(_localizer["Firebase register return null"]);
                    }

                    var user = new User
                    {
                        Email = request.Email,
                        Username = request.Username,
                        RoleId = (int)request.RoleId, // Luôn mặc định là User
                        IdentityId = identityId,
                        IsActive = true,
                    };

                    await _userRepos.AddAsync(user, cancellationToken);
                    await _unitOfWork.SaveChangesAsync(cancellationToken);

                    return CommandResult<Result>.Success(new Result
                    {
                        Message = _localizer["User created successfully"],
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