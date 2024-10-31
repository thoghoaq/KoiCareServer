using FluentValidation;
using KoiCare.Application.Abtractions.Database;
using KoiCare.Application.Abtractions.Localization;
using KoiCare.Application.Abtractions.LoggedUser;
using KoiCare.Application.Commons;
using KoiCare.Application.Features.Koifish;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace KoiCare.Application.Features.Feeding
{
    public class UpdateFeedingSchedule
    {
        public class Command : IRequest<CommandResult<Result>>
        {
            public int Id { get; set; }
            public int PondId { get; set; }
            public decimal Amount { get; set; }
            public decimal Period { get; set; }
        }

        public class Result
        {
            public string? Message { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator(IAppLocalizer localizer)
            {
                RuleFor(x => x.Id).NotEmpty();
                RuleFor(x => x.PondId).NotEmpty();
                RuleFor(x => x.Amount).NotEmpty().GreaterThan(0).WithMessage(localizer["Amount must be greater than 0"]);
                RuleFor(x => x.Period).NotEmpty().GreaterThan(0).WithMessage(localizer["Period must be greater than 0"]);
            }
        }

        public class Handler(
        IAppLocalizer localizer,
        ILogger<UpdateKoiFish> logger,
        ILoggedUser loggedUser,
        IUnitOfWork unitOfWork,
        IRepository<Domain.Entities.FeedingSchedule> feedingScheduleRepos
        ) : BaseRequestHandler<Command, CommandResult<Result>>(localizer, logger, loggedUser, unitOfWork)
        {
            public override async Task<CommandResult<Result>> Handle(Command request, CancellationToken cancellationToken)
            {
                try
                {
                    var validator = new CommandValidator(localizer);
                    var validationResult = await validator.ValidateAsync(request, cancellationToken);
                    if (!validationResult.IsValid)
                    {
                        return CommandResult<Result>.Fail(validationResult.Errors.FirstOrDefault()?.ErrorMessage ?? "Error in validation");
                    }
                    var feedingSchedule = await feedingScheduleRepos.Queryable().FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
                    if (feedingSchedule == null)
                    {
                        return CommandResult<Result>.Fail(localizer["Feeding schedule not found"]);
                    }
                    feedingSchedule.PondId = request.PondId;
                    feedingSchedule.Amount = request.Amount;
                    feedingSchedule.Period = request.Period;

                    await _unitOfWork.SaveChangesAsync(cancellationToken);

                    return CommandResult<Result>.Success(new Result
                    {
                        Message = _localizer["Feeding schedule updated successfully"]
                    });
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error updating feeding schedule");
                    return CommandResult<Result>.Fail(localizer["Error updating feeding schedule"]);
                }
            }
        }
    }
}
