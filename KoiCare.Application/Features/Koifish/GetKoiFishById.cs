using KoiCare.Application.Abtractions.Database;
using KoiCare.Application.Abtractions.Localization;
using KoiCare.Application.Abtractions.LoggedUser;
using KoiCare.Application.Commons;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace KoiCare.Application.Features.Koifish
{
    public class GetKoiFishById
    {
        public class Result
        {
            public required int Id { get; set; }
            public string? Name { get; set; }
            public int? KoiTypeId { get; set; }
            public string? KoiType { get; set; }
            public int? PondId { get; set; }
            public string? PondName { get; set; }
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
            public int Id { get; set; }
        }

        public class Handler(
            IAppLocalizer localizer,
            ILogger<GetKoiFishById> logger,
            ILoggedUser loggedUser,
            IUnitOfWork unitOfWork,
            IRepository<Domain.Entities.KoiIndividual> koiRepos
            ) : BaseRequestHandler<Query, CommandResult<Result>>(localizer, logger, loggedUser, unitOfWork)
        {
            public override async Task<CommandResult<Result>> Handle(Query request, CancellationToken cancellationToken)
            {
                var koifish = await koiRepos.Queryable()
                    .Include(x => x.KoiType)
                    .Include(x => x.Pond)
                    .Include(x => x.KoiGrowths)
                    .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

                if (koifish == null)
                {
                    return CommandResult<Result>.Fail("Koi fish not found");
                }

                var result = new Result
                {
                    Id = koifish.Id,
                    Name = koifish.Name,
                    KoiTypeId = koifish.KoiTypeId,
                    KoiType = koifish.KoiType.Name,
                    PondId = koifish.PondId,
                    PondName = koifish.Pond.Name,
                    ImageUrl = koifish.ImageUrl,
                    Age = koifish.Age,
                    Length = koifish.KoiGrowths.OrderByDescending(x => x.MeasuredAt).FirstOrDefault()?.Length ?? koifish.Length,
                    Weight = koifish.KoiGrowths.OrderByDescending(x => x.MeasuredAt).FirstOrDefault()?.Weight ?? koifish.Weight,
                    Gender = koifish.Gender,
                    Origin = koifish.Origin,
                    Shape = koifish.Shape,
                    Breed = koifish.Breed,
                };

                return CommandResult<Result>.Success(result);
            }
        }
    }
}
