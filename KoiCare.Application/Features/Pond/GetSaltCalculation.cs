using KoiCare.Application.Abtractions.Database;
using KoiCare.Application.Abtractions.Localization;
using KoiCare.Application.Abtractions.LoggedUser;
using KoiCare.Application.Commons;
using KoiCare.Domain.Constants;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;

namespace KoiCare.Application.Features.Pond
{
    public class GetSaltCalculation
    {
        public class Query : IRequest<CommandResult<Result>>
        {
            [Required]
            public int PondId { get; set; }
        }

        public class Result
        {
            public decimal WaterVolume { get; set; }
            public decimal TreatmentAmount { get; set; }
            public decimal WaterImprovementAmount { get; set; }
        }

        public class Handler(IAppLocalizer localizer, ILogger<GetSaltCalculation> logger, ILoggedUser loggedUser, IUnitOfWork unitOfWork,
            IRepository<Domain.Entities.Pond> pondRepos) : BaseRequestHandler<Query, CommandResult<Result>>(localizer, logger, loggedUser, unitOfWork)
        {
            public async override Task<CommandResult<Result>> Handle(Query request, CancellationToken cancellationToken)
            {
                var pond = await pondRepos.Queryable()
                    .FirstOrDefaultAsync(x => x.Id == request.PondId, cancellationToken);
                if (pond == null)
                {
                    return CommandResult<Result>.Fail(_localizer["Pond not found"]);
                }

                return CommandResult<Result>.Success(new Result
                {
                    WaterVolume = pond.Volume,
                    TreatmentAmount = pond.Volume * CWaterParameter.TreatmentPercent,
                    WaterImprovementAmount = pond.Volume * CWaterParameter.WaterImprovementPercent
                });
            }
        }
    }
}
