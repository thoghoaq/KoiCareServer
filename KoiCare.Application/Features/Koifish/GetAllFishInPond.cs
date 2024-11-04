using KoiCare.Application.Abtractions.Database;
using KoiCare.Application.Abtractions.Localization;
using KoiCare.Application.Abtractions.LoggedUser;
using KoiCare.Application.Commons;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace KoiCare.Application.Features.Koifish
{
    public class GetAllFishInPond
    {
        public record KoiResult
        {
            public int Id { get; set; }
            public int KoiTypeId { get; set; }
            public string? KoiType { get; set; }
            public int PondId { get; set; }
            public string? PondName { get; set; }
            public required string Name { get; set; }
            public string? ImageUrl { get; set; }
            public decimal? Age { get; set; }
            public decimal? Length { get; set; }
            public decimal? Weight { get; set; }
            public int? Gender { get; set; }
            public string? Origin { get; set; }
            public int? Shape { get; set; }
            public string? Breed { get; set; }
        }

        public class Query : IRequest<CommandResult<Result>>
        {
            public int PondId { get; set; }
        }

        public class Result
        {
            public List<KoiResult> KoiResults { get; set; } = [];
        }

        public class Handler(
            IAppLocalizer localizer,
            ILogger<GetAllFishInPond> logger,
            ILoggedUser loggedUser,
            IUnitOfWork unitOfWork,
            IRepository<Domain.Entities.KoiIndividual> koiRepos
            ) : BaseRequestHandler<Query, CommandResult<Result>>(localizer, logger, loggedUser, unitOfWork)
        {
            public override Task<CommandResult<Result>> Handle(Query request, CancellationToken cancellationToken)
            {
                var query = koiRepos.Queryable()
                    .Include(x => x.Pond)
                    .Include(x => x.KoiType)
                    .Include(x => x.KoiGrowths)
                    .Where(x => x.PondId.Equals(request.PondId))
                    .AsEnumerable()
                    .Select(x =>
                    {
                        var growth = x.KoiGrowths.OrderByDescending(y => y.MeasuredAt).FirstOrDefault();
                        return new KoiResult
                        {
                            Id = x.Id,
                            Name = x.Name,
                            KoiTypeId = x.KoiTypeId,
                            KoiType = x.KoiType.Name,
                            PondId = x.PondId,
                            PondName = x.Pond.Name,
                            ImageUrl = x.ImageUrl,
                            Gender = x.Gender,
                            Weight = growth?.Weight ?? x.Weight,
                            Age = x.Age,
                            Length = growth?.Length ?? x.Length,
                            Origin = x.Origin,
                            Shape = x.Shape,
                            Breed = x.Breed,
                        };
                    });

                // Trả về kết quả
                return Task.FromResult(CommandResult<Result>.Success(new Result
                {
                    KoiResults = query.ToList()
                }));
            }
        }
    }
}
