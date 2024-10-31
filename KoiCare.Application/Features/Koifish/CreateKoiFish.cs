using KoiCare.Application.Abtractions.Database;
using KoiCare.Application.Abtractions.Localization;
using KoiCare.Application.Abtractions.LoggedUser;
using KoiCare.Application.Commons;
using KoiCare.Application.Helpers;
using KoiCare.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Net;

namespace KoiCare.Application.Features.Koifish
{
    public class CreateKoiFish
    {
        public class Command : IRequest<CommandResult<Result>>
        {
            public required string Name { get; set; }
            public required int KoiTypeId { get; set; }
            public required int PondId { get; set; }
            public string? ImageUrl { get; set; }
            public decimal Age { get; set; }
            public decimal? Weight { get; set; }
            public EGender? Gender { get; set; }
            public string? Origin { get; set; }
            public int? Shape { get; set; }
            public string? Breed { get; set; }
            public decimal? Length { get; set; }
        }

        public class Result
        {
            public string? Message { get; set; }
        }

        public class Handler(
           IRepository<Domain.Entities.KoiIndividual> koiRepos,
           IRepository<Domain.Entities.Pond> pondRepos,
           IAppLocalizer localizer,
           ILogger<CreateKoiFish> logger,
           ILoggedUser loggedUser,
           IUnitOfWork unitOfWork
        ) : BaseRequestHandler<Command, CommandResult<Result>>(localizer, logger, loggedUser, unitOfWork)
        {
            private readonly IRepository<Domain.Entities.KoiIndividual> _koiRepos = koiRepos;

            public override async Task<CommandResult<Result>> Handle(Command request, CancellationToken cancellationToken)
            {
                try
                {
                    var pond = await pondRepos.Queryable()
                        .Include(x => x.KoiGroup).ThenInclude(x => x!.KoiTypes)
                        .FirstOrDefaultAsync(x => x.Id == request.PondId, cancellationToken);
                    if (pond == null)
                    {
                        return CommandResult<Result>.Fail(_localizer["Pond not found"]);
                    }
                    if (pond.KoiGroup != null && !pond.KoiGroup!.KoiTypes.Any(t => t.Id == request.KoiTypeId))
                    {
                        return CommandResult<Result>.Fail(_localizer["Koi type is not suitable for this pond"]);
                    }

                    if (pond.Gender != null && request.Gender != pond.Gender)
                    {
                        return CommandResult<Result>.Fail(_localizer["Gender is not suitable for this pond"]);
                    }

                    if (pond.AgeRange != null && !request.Age.IsInAgeRange(pond.AgeRange))
                    {
                        return CommandResult<Result>.Fail(_localizer["Age is not suitable for this pond"]);
                    }

                    var koifish = new Domain.Entities.KoiIndividual()
                    {
                        Name = request.Name,
                        KoiTypeId = request.KoiTypeId,
                        PondId = request.PondId,
                        ImageUrl = request.ImageUrl,
                        Age = request.Age,
                        Weight = request.Weight,
                        Gender = (int?)request.Gender,
                        Origin = request.Origin,
                        Shape = request.Shape,
                        Length = request.Length,
                        Breed = request.Breed,
                    };

                    await _koiRepos.AddAsync(koifish, cancellationToken);
                    await _unitOfWork.SaveChangesAsync(cancellationToken);

                    return CommandResult<Result>.Success(new Result
                    {
                        Message = _localizer["Koi fish created successfully"],
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
