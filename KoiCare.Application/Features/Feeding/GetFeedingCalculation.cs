using KoiCare.Application.Abtractions.Database;
using KoiCare.Application.Abtractions.Localization;
using KoiCare.Application.Abtractions.LoggedUser;
using KoiCare.Application.Commons;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace KoiCare.Application.Features.Feeding
{
    public class GetFeedingCalculation
    {
        public class Query : IRequest<CommandResult<QueryResult>>
        {
            public int KoiIndividualId { get; set; }
        }

        public class QueryResult
        {
            public int Id { get; set; }
            public decimal Amount { get; set; }
            public decimal DailyAmount { get; set; }
            public decimal Frequency { get; set; }
            public int Type { get; set; }
        }

        public class Handler(
        IAppLocalizer localizer,
        ILogger<CalculateFeedingAmount> logger,
        ILoggedUser loggedUser,
        IUnitOfWork unitOfWork,
        IRepository<Domain.Entities.FeedCalculation> feedCalculationRepos
        ) : BaseRequestHandler<Query, CommandResult<QueryResult>>(localizer, logger, loggedUser, unitOfWork)
        {
            public override async Task<CommandResult<QueryResult>> Handle(Query request, CancellationToken cancellationToken)
            {
                var feedCalculation = await feedCalculationRepos.Queryable().FirstOrDefaultAsync(x => x.KoiIndividualId == request.KoiIndividualId, cancellationToken);

                if (feedCalculation == null)
                {
                    return CommandResult<QueryResult>.Fail(_localizer["Feeding calculation not found"]);
                }

                return CommandResult<QueryResult>.Success(new QueryResult
                {
                    Id = feedCalculation.Id,
                    Amount = feedCalculation.DailyAmount * feedCalculation.Frequency,
                    DailyAmount = feedCalculation.DailyAmount,
                    Frequency = feedCalculation.Frequency,
                    Type = feedCalculation.Type
                });
            }
        }
    }
}
