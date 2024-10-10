using KoiCare.Application.Abtractions.Database;
using KoiCare.Application.Abtractions.Localization;
using KoiCare.Application.Abtractions.LoggedUser;
using KoiCare.Application.Commons;
using KoiCare.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace KoiCare.Application.Features.Pond
{
    public class GetListPonds
    {
        public class Query : IRequest<CommandResult<Result>>
        {

        }

        public class Result
        {
            public List<PondResult> Ponds { get; set; } = [];
        }

        public class PondResult
        {
            public int Id { get; set; }
            public int OwnerId { get; set; }
            public required string Name { get; set; }
            public string? ImageUrl { get; set; }
            public decimal Length { get; set; }
            public decimal Width { get; set; }
            public decimal Depth { get; set; }
            public decimal Volume { get; set; }
            public decimal DrainageCount { get; set; }
            public decimal PumpCapacity { get; set; }
        }

        public class Handler(
            IRepository<Domain.Entities.Pond> pondRepos,
            IAppLocalizer localizer,
            ILogger<GetListPonds> logger,
            ILoggedUser loggedUser,
            IUnitOfWork unitOfWork) : BaseRequestHandler<Query, CommandResult<Result>>(localizer, logger, loggedUser, unitOfWork)
        {
            public async override Task<CommandResult<Result>> Handle(Query request, CancellationToken cancellationToken)
            {
                // get ponds is owned by the logged user, if admin get all ponds
                var ponds = await pondRepos.Queryable()
                    .Where(x => _loggedUser.RoleId == (int)ERole.Admin || x.OwnerId == _loggedUser.UserId)
                    .Select(x => new PondResult
                    {
                        Id = x.Id,
                        OwnerId = x.OwnerId,
                        Name = x.Name,
                        ImageUrl = x.ImageUrl,
                        Length = x.Length,
                        Width = x.Width,
                        Depth = x.Depth,
                        Volume = x.Volume,
                        DrainageCount = x.DrainageCount,
                        PumpCapacity = x.PumpCapacity
                    }).ToListAsync(cancellationToken);

                return CommandResult<Result>.Success(new Result
                {
                    Ponds = ponds
                });
            }
        }
    }
}
