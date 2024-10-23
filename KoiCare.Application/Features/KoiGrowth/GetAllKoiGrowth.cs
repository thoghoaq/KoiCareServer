using KoiCare.Application.Abtractions.Database;
using KoiCare.Application.Abtractions.Localization;
using KoiCare.Application.Abtractions.LoggedUser;
using KoiCare.Application.Commons;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace KoiCare.Application.Features.KoiGrowth
{
    public class GetAllKoiGrowth
    {
        public record KoiGrowthResult
        {
            public int Id { get; set; }
            public int KoiIndividualId { get; set; }
            public string? KoiName { get; set; }
            public decimal Length { get; set; }
            public decimal Weight { get; set; }
            public DateTime MeasuredAt { get; set; }
        }

        public class Query : IRequest<CommandResult<Result>>
        {
            public int KoiIndividualId { get; set; }
        }

        public class Result
        {
            public List<KoiGrowthResult> KoiGrowthResults { get; set; } = [];
        }

        public class Handler(
            IAppLocalizer localizer,
            ILogger<GetAllKoiGrowth> logger,
            ILoggedUser loggedUser,
            IUnitOfWork unitOfWork,
            IRepository<Domain.Entities.KoiGrowth> koiRepos
            ) : BaseRequestHandler<Query, CommandResult<Result>>(localizer, logger, loggedUser, unitOfWork)
        {
            public override Task<CommandResult<Result>> Handle(Query request, CancellationToken cancellationToken)
            {
                var query = koiRepos.Queryable()
                    .Include(x => x.KoiIndividual)
                    .Where(x => x.KoiIndividualId.Equals(request.KoiIndividualId))
                    .Select(x => new KoiGrowthResult
                    {
                        Id = x.Id,
                        KoiIndividualId = x.KoiIndividualId,
                        KoiName = x.KoiIndividual.Name,
                        Length = x.Length,
                        Weight = x.Weight,
                        MeasuredAt = x.MeasuredAt,
                    });

                // Trả về kết quả
                return Task.FromResult(CommandResult<Result>.Success(new Result
                {
                    KoiGrowthResults = query.ToList()
                }));
            }
        }
    }
}
