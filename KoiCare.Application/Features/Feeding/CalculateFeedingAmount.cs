using FluentValidation;
using KoiCare.Application.Abtractions.Database;
using KoiCare.Application.Abtractions.Localization;
using KoiCare.Application.Abtractions.LoggedUser;
using KoiCare.Application.Commons;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Net;

namespace KoiCare.Application.Features.Feeding
{
    public class CalculateFeedingAmount
    {
        public class Command : IRequest<CommandResult<Result>>
        {
            public int KoiIndividualId { get; set; }
            public decimal DailyAmount { get; set; }
            public decimal Frequency { get; set; }
            public int Type { get; set; }
        }

        public class Result
        {
            public string? Message { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator(IAppLocalizer localizer)
            {
                RuleFor(x => x.KoiIndividualId).NotEmpty();
                RuleFor(x => x.DailyAmount).NotEmpty().GreaterThan(0).WithMessage(localizer["Daily amount must be greater than 0"]);
                RuleFor(x => x.Frequency).NotEmpty().GreaterThan(0).WithMessage(localizer["Frequency must be greater than 0"]);
                RuleFor(x => x.Type).NotEmpty();
            }
        }

        public class Handler(
        IAppLocalizer localizer,
        ILogger<CalculateFeedingAmount> logger,
        ILoggedUser loggedUser,
        IUnitOfWork unitOfWork,
        IRepository<Domain.Entities.FeedCalculation> feedCalculationRepos
        ) : BaseRequestHandler<Command, CommandResult<Result>>(localizer, logger, loggedUser, unitOfWork)
        {
            public override async Task<CommandResult<Result>> Handle(Command request, CancellationToken cancellationToken)
            {
                try
                {
                    var validator = new CommandValidator(_localizer);
                    var validationResult = await validator.ValidateAsync(request, cancellationToken);
                    if (!validationResult.IsValid)
                    {
                        return CommandResult<Result>.Fail(validationResult.Errors.FirstOrDefault()?.ErrorMessage ?? "Error in validation");
                    }
                    var feedCalculation = new Domain.Entities.FeedCalculation()
                    {
                        KoiIndividualId = request.KoiIndividualId,
                        DailyAmount = request.DailyAmount,
                        Frequency = request.Frequency,
                        Type = request.Type,
                        CalculationDate = DateTime.UtcNow,
                    };

                    await feedCalculationRepos.AddAsync(feedCalculation, cancellationToken);
                    await _unitOfWork.SaveChangesAsync(cancellationToken);

                    return CommandResult<Result>.Success(new Result
                    {
                        Message = _localizer["Feed calculation created successfully"]
                    });
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "CalculateFeedingAmountError");
                    return CommandResult<Result>.Fail(HttpStatusCode.InternalServerError, _localizer["Error creating feed calculation"]);
                }
            }
        }
    }
}
