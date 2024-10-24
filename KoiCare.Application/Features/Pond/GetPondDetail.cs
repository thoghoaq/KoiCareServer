using KoiCare.Application.Abtractions.Database;
using KoiCare.Application.Abtractions.Localization;
using KoiCare.Application.Abtractions.LoggedUser;
using KoiCare.Application.Commons;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace KoiCare.Application.Features.Pond
{
    public class GetPondDetail
    {
        public class Query : IRequest<CommandResult<Result>>
        {
            [Required]
            public int Id { get; set; }
        }

        public class Result
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
            public virtual SaltRequirement? SaltRequirement { get; set; } = null!;
            public virtual ICollection<WaterParameter> WaterParameters { get; set; } = [];
        }

        public record SaltRequirement
        {
            public int Id { get; set; }
            public int PondId { get; set; }
            public decimal RequiredAmount { get; set; }
        }

        public record WaterParameter
        {
            public int Id { get; set; }
            public int PondId { get; set; }
            public decimal? Temperature { get; set; }
            public decimal? Salinity { get; set; }
            public decimal? Ph { get; set; }
            public decimal? Oxygen { get; set; }
            public decimal? NO2 { get; set; }
            public decimal? NO3 { get; set; }
            public decimal? PO4 { get; set; }
            public DateTime MeasuredAt { get; set; }
        }

        public class Handler(
            IRepository<Domain.Entities.Pond> pondRepos,
            IAppLocalizer localizer,
            ILogger<GetPondDetail> logger,
            ILoggedUser
            loggedUser,
            IUnitOfWork unitOfWork) : BaseRequestHandler<Query, CommandResult<Result>>(localizer, logger, loggedUser, unitOfWork)
        {
            public async override Task<CommandResult<Result>> Handle(Query request, CancellationToken cancellationToken)
            {
                var pond = await pondRepos.Queryable()
                    .Include(x => x.SaltRequirement)
                    .Include(x => x.WaterParameters)
                    .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
                if (pond == null)
                {
                    return CommandResult<Result>.Fail(HttpStatusCode.NotFound, _localizer["Pond not found"]);
                }

                if (_loggedUser.UserId != pond.OwnerId)
                {
                    return CommandResult<Result>.Fail(HttpStatusCode.Forbidden, _localizer["You are not owner of this pond"]);
                }

                return CommandResult<Result>.Success(new Result
                {
                    Id = pond.Id,
                    OwnerId = pond.OwnerId,
                    Name = pond.Name,
                    ImageUrl = pond.ImageUrl,
                    Length = pond.Length,
                    Width = pond.Width,
                    Depth = pond.Depth,
                    Volume = pond.Volume,
                    DrainageCount = pond.DrainageCount,
                    PumpCapacity = pond.PumpCapacity,
                    SaltRequirement = pond.SaltRequirement != null ? new SaltRequirement
                    {
                        Id = pond.SaltRequirement.Id,
                        PondId = pond.SaltRequirement.PondId,
                        RequiredAmount = pond.SaltRequirement.RequiredAmount
                    } : null,
                    WaterParameters = pond.WaterParameters.OrderByDescending(y => y.MeasuredAt).Select(w => new WaterParameter
                    {
                        Id = w.Id,
                        PondId = w.PondId,
                        Temperature = w.Temperature,
                        Salinity = w.Salinity,
                        Ph = w.Ph,
                        Oxygen = w.Oxygen,
                        NO2 = w.NO2,
                        NO3 = w.NO3,
                        PO4 = w.PO4,
                        MeasuredAt = w.MeasuredAt
                    }).ToList()
                });
            }
        }
    }
}
