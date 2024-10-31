using FluentValidation;
using KoiCare.Application.Abtractions.Database;
using KoiCare.Application.Abtractions.Localization;
using KoiCare.Application.Abtractions.LoggedUser;
using KoiCare.Application.Commons;
using KoiCare.Application.Features.Koifish;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Net;

namespace KoiCare.Application.Features.Feeding
{
    public class CreateFeedingSchedule
    {
        public class Command : IRequest<CommandResult<Result>>
        {
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
                    var feedingSchedule = new Domain.Entities.FeedingSchedule()
                    {
                        PondId = request.PondId,
                        Amount = request.Amount,
                        Period = request.Period
                    };

                    await feedingScheduleRepos.AddAsync(feedingSchedule, cancellationToken);
                    await _unitOfWork.SaveChangesAsync(cancellationToken);

                    return CommandResult<Result>.Success(new Result
                    {
                        Message = _localizer["Feeding schedule created successfully"]
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
