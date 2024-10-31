using KoiCare.Application.Abtractions.Database;
using KoiCare.Application.Abtractions.Localization;
using KoiCare.Application.Abtractions.LoggedUser;
using KoiCare.Application.Commons;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace KoiCare.Application.Features.KoiGroup
{
    public class GetKoiGroups
    {
        public class Query : IRequest<CommandResult<Result>>
        {
            public bool? IncludeKoiTypes { get; set; } = false;
        }

        public class Result
        {
            public List<KoiGroup> KoiGroups { get; set; } = [];
        }

        public record KoiGroup
        {
            public int Id { get; set; }
            public required string Name { get; set; }
            public List<KoiType> KoiTypes { get; set; } = [];
        }

        public record KoiType
        {
            public int Id { get; set; }
            public required string Name { get; set; }
            public string? Description { get; set; }
        }

        public class Handler(IAppLocalizer localizer, ILogger<GetKoiGroups> logger, ILoggedUser loggedUser, IUnitOfWork unitOfWork, IRepository<Domain.Entities.KoiGroup> koiGroupRepos) : BaseRequestHandler<Query, CommandResult<Result>>(localizer, logger, loggedUser, unitOfWork)
        {
            public async override Task<CommandResult<Result>> Handle(Query request, CancellationToken cancellationToken)
            {
                var query = koiGroupRepos.Queryable();
                return CommandResult<Result>.Success(new Result
                {
                    KoiGroups = await query.Select(x => new KoiGroup
                    {
                        Id = x.Id,
                        Name = x.Name,
                        KoiTypes = request.IncludeKoiTypes == true ? x.KoiTypes.Select(y => new KoiType
                        {
                            Id = y.Id,
                            Name = y.Name,
                            Description = y.Description,
                        }).ToList() : new List<KoiType>()
                    }).ToListAsync()
                });
            }
        }
    }
}
