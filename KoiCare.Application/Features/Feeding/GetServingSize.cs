using KoiCare.Application.Abtractions.Database;
using KoiCare.Application.Abtractions.Localization;
using KoiCare.Application.Abtractions.LoggedUser;
using KoiCare.Application.Commons;
using KoiCare.Application.Helpers;
using KoiCare.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;

namespace KoiCare.Application.Features.Feeding
{
    public class GetServingSize
    {
        public class Query : IRequest<CommandResult<Result>>
        {
            [Required]
            public int PondId { get; set; }
        }

        public class Result
        {
            public int Id { get; set; }
            public EAgeRange AgeRange { get; set; }
            public string? AgeRangeDescription => AgeRange.GetDisplayName();
            public decimal WeightPercent { get; set; }
            public required string FoodDescription { get; set; }
            public required string DailyFrequency { get; set; }
            public required int KoiGroupId { get; set; }
        }

        public class Handler(
            IAppLocalizer localizer,
            ILogger<GetServingSize> logger,
            ILoggedUser loggedUser,
            IUnitOfWork unitOfWork,
            IRepository<Domain.Entities.Pond> pondRepos,
            IRepository<Domain.Entities.ServingSize> servingSizeRepos
            ) : BaseRequestHandler<Query, CommandResult<Result>>(localizer, logger, loggedUser, unitOfWork)
        {
            public async override Task<CommandResult<Result>> Handle(Query request, CancellationToken cancellationToken)
            {
                var pond = await pondRepos.Queryable().FirstOrDefaultAsync(x => x.Id == request.PondId, cancellationToken);
                if (pond == null)
                {
                    return CommandResult<Result>.Fail(_localizer["Pond not found"]);
                }
                var servingSize = await servingSizeRepos.Queryable().FirstOrDefaultAsync(x => x.KoiGroupId == pond.KoiGroupId && x.AgeRange == pond.AgeRange, cancellationToken);
                if (servingSize == null)
                {
                    return CommandResult<Result>.Fail(_localizer["There are no serving size match with pond setting"]);
                }
                return CommandResult<Result>.Success(new Result
                {
                    Id = servingSize.Id,
                    AgeRange = servingSize.AgeRange,
                    WeightPercent = servingSize.WeightPercent,
                    FoodDescription = servingSize.FoodDescription,
                    DailyFrequency = servingSize.DailyFrequency,
                    KoiGroupId = servingSize.KoiGroupId
                });
            }
        }
    }
}
