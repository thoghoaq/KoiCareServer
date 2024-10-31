using KoiCare.Application.Abtractions.Database;
using KoiCare.Application.Abtractions.Localization;
using KoiCare.Application.Abtractions.LoggedUser;
using KoiCare.Application.Commons;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Net;

namespace KoiCare.Application.Features.Feeding
{
    public class GetFeedingSchedule
    {
        public class Query : IRequest<CommandResult<Result>>
        {
            public int PondId { get; set; }
        }

        public class Result
        {
            public int Id { get; set; }
            public decimal Amount { get; set; }
            public decimal Period { get; set; }
        }

        public class Handler(
            IRepository<Domain.Entities.FeedingSchedule> feedingScheduleRepos,
            IAppLocalizer localizer,
            ILogger<GetFeedingSchedule> logger,
            ILoggedUser loggedUser,
            IUnitOfWork unitOfWork
        ) : BaseRequestHandler<Query, CommandResult<Result>>(localizer, logger, loggedUser, unitOfWork)
        {
            private readonly IRepository<Domain.Entities.FeedingSchedule> _feedingScheduleRepos = feedingScheduleRepos;

            public override async Task<CommandResult<Result>> Handle(Query request, CancellationToken cancellationToken)
            {
                try
                {
                    var feedingSchedule = await _feedingScheduleRepos.Queryable()
                        .Where(x => x.PondId == request.PondId).FirstOrDefaultAsync(cancellationToken);
                    if (feedingSchedule == null)
                    {
                        return CommandResult<Result>.Fail(localizer["Feeding schedule not found"]);
                    }

                    return CommandResult<Result>.Success(new Result
                    {
                        Id = feedingSchedule.Id,
                        Amount = feedingSchedule.Amount,
                        Period = feedingSchedule.Period
                    });
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Error getting feeding schedule");
                    return CommandResult<Result>.Fail(HttpStatusCode.InternalServerError, localizer["Error getting feeding schedule"]);
                }
            }
        }
    }
}
