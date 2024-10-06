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
    public class GetAllUser
    {
        public class Query : IRequest<CommandResult<Result>>
        {
            public int? PageNumber { get; set; }
            public int? PageSize { get; set; }
            public string? Search { get; set; }
            public ERole? Role { get; set; }
        }

        public class Result
        {
            public List<UserResult> Users { get; set; } = [];
        }

        public record UserResult
        {
            public int Id { get; set; }
            public int RoleId { get; set; }
            public string? RoleName { get; set; }
            public required string Username { get; set; }
            public required string Email { get; set; }
            public required string IdentityId { get; set; }
            public bool IsActive { get; set; }
        }

        public class Handler(
            IAppLocalizer localizer,
            ILogger<GetAllUser> logger,
            ILoggedUser loggedUser,
            IUnitOfWork unitOfWork,
            IRepository<User> userRepos
            ) : BaseRequestHandler<Query, CommandResult<Result>>(localizer, logger, loggedUser, unitOfWork)
        {
            public override Task<CommandResult<Result>> Handle(Query request, CancellationToken cancellationToken)
            {
                var query = userRepos.Queryable()
                    .Include(x => x.Role)
                    .Where(x => request.Search == null || x.Username.Contains(request.Search!))
                    .Where(x => request.Role == null || x.RoleId == (int)request.Role!)
                    .Select(x => new UserResult
                    {
                        Id = x.Id,
                        Email = x.Email,
                        Username = x.Username,
                        RoleId = x.RoleId,
                        RoleName = x.Role.Name,
                        IdentityId = x.IdentityId,
                        IsActive = x.IsActive
                    });

                if (request.PageNumber.HasValue && request.PageSize.HasValue)
                {
                    query = query.Skip((request.PageNumber.Value - 1) * request.PageSize.Value).Take(request.PageSize.Value);
                }

                return Task.FromResult(CommandResult<Result>.Success(new Result
                {
                    Users = query.ToList()
                }));
            }
        }
    }
}
