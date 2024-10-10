using FluentValidation;
using KoiCare.Application.Abtractions.Database;
using KoiCare.Application.Abtractions.Localization;
using KoiCare.Application.Abtractions.LoggedUser;
using KoiCare.Application.Commons;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Net;

namespace KoiCare.Application.Features.Pond
{
    public class CreateUpdatePond
    {
        public class Command : IRequest<CommandResult<Result>>
        {
            public int? Id { get; set; }
            public string? Name { get; set; }
            public string? ImageUrl { get; set; }
            public decimal? Length { get; set; }
            public decimal? Width { get; set; }
            public decimal? Depth { get; set; }
            public decimal? Volume { get; set; }
            public decimal? DrainageCount { get; set; }
            public decimal? PumpCapacity { get; set; }
        }

        public class CreateCommandValidator : AbstractValidator<Command>
        {
            public CreateCommandValidator(IAppLocalizer localizer)
            {
                RuleFor(x => x.Name)
                    .NotNull().WithMessage(localizer["Name is required"])
                    .NotEmpty().WithMessage(localizer["Name is required"]);
                RuleFor(x => x.Length)
                    .NotNull().WithMessage(localizer["Length is required"])
                    .NotEmpty().WithMessage(localizer["Length is required"])
                    .GreaterThan(0).WithMessage(localizer["Length is greater than 0"]);
                RuleFor(x => x.Width)
                    .NotNull().WithMessage(localizer["Width is required"])
                    .NotEmpty().WithMessage(localizer["Width is required"])
                    .GreaterThan(0).WithMessage(localizer["Width is greater than 0"]);
                RuleFor(x => x.Depth)
                    .NotNull().WithMessage(localizer["Depth is required"])
                    .NotEmpty().WithMessage(localizer["Depth is required"])
                    .GreaterThan(0).WithMessage(localizer["Depth is greater than 0"]);
                RuleFor(x => x.Volume)
                    .NotNull().WithMessage(localizer["Volume is required"])
                    .NotEmpty().WithMessage(localizer["Volume is required"])
                    .GreaterThan(0).WithMessage(localizer["Volume is greater than 0"]);
                RuleFor(x => x.DrainageCount)
                    .NotNull().WithMessage(localizer["Drainage count is required"])
                    .NotEmpty().WithMessage(localizer["Drainage count is required"])
                    .GreaterThan(0).WithMessage(localizer["Drainage count is greater than 0"]);
                RuleFor(x => x.PumpCapacity)
                    .NotNull().WithMessage(localizer["Pump capacity is required"])
                    .NotEmpty().WithMessage(localizer["Pump capacity is required"])
                    .GreaterThan(0).WithMessage(localizer["Pump capacity is greater than 0"]);
            }
        }

        public class UpdateCommandValidator : AbstractValidator<Command>
        {
            public UpdateCommandValidator(IAppLocalizer localizer)
            {
                RuleFor(x => x.Id).NotEmpty().WithMessage(localizer["Id is required"]);
                RuleFor(x => x.Name).NotEmpty().WithMessage(localizer["Name is required"]);
                RuleFor(x => x.Length)
                    .NotEmpty().WithMessage(localizer["Length is required"])
                    .GreaterThan(0).WithMessage(localizer["Length is greater than 0"]);
                RuleFor(x => x.Width)
                    .NotEmpty().WithMessage(localizer["Width is required"])
                    .GreaterThan(0).WithMessage(localizer["Width is greater than 0"]);
                RuleFor(x => x.Depth)
                    .NotEmpty().WithMessage(localizer["Depth is required"])
                    .GreaterThan(0).WithMessage(localizer["Depth is greater than 0"]);
                RuleFor(x => x.Volume)
                    .NotEmpty().WithMessage(localizer["Volume is required"])
                    .GreaterThan(0).WithMessage(localizer["Volume is greater than 0"]);
                RuleFor(x => x.DrainageCount)
                    .NotEmpty().WithMessage(localizer["Drainage count is required"])
                    .GreaterThan(0).WithMessage(localizer["Drainage count is greater than 0"]);
                RuleFor(x => x.PumpCapacity)
                    .NotEmpty().WithMessage(localizer["Pump capacity is required"])
                    .GreaterThan(0).WithMessage(localizer["Pump capacity is greater than 0"]);
            }
        }

        public class Result
        {
            public int Id { get; set; }
            public string? Message { get; set; }
        }

        public class Handler(
            IRepository<Domain.Entities.Pond> pondRepos,
            IAppLocalizer localizer,
            ILogger<CreateUpdatePond> logger,
            ILoggedUser loggedUser,
            IUnitOfWork unitOfWork) : BaseRequestHandler<Command, CommandResult<Result>>(localizer, logger, loggedUser, unitOfWork)
        {
            public async override Task<CommandResult<Result>> Handle(Command request, CancellationToken cancellationToken)
            {
                try
                {
                    var ownerId = _loggedUser.UserId;
                    int pondId;
                    if (request.Id.HasValue)
                    {
                        // Update
                        var validator = new UpdateCommandValidator(localizer);
                        var validationResult = await validator.ValidateAsync(request, cancellationToken);
                        if (!validationResult.IsValid)
                        {
                            return CommandResult<Result>.Fail(validationResult.Errors.FirstOrDefault()?.ErrorMessage ?? "Error in validation");
                        }

                        var pond = await pondRepos.Queryable().FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
                        if (pond == null)
                        {
                            return CommandResult<Result>.Fail(HttpStatusCode.NotFound, _localizer["Pond not found"]);
                        }
                        if (pond.OwnerId != ownerId)
                        {
                            return CommandResult<Result>.Fail(HttpStatusCode.Forbidden, _localizer["You are not allowed to update this pond"]);
                        }
                        if (request.Name != null) pond.Name = request.Name;
                        if (request.ImageUrl != null) pond.ImageUrl = request.ImageUrl;
                        if (request.Length.HasValue) pond.Length = request.Length.Value;
                        if (request.Width.HasValue) pond.Width = request.Width.Value;
                        if (request.Depth.HasValue) pond.Depth = request.Depth.Value;
                        if (request.Volume.HasValue) pond.Volume = request.Volume.Value;
                        if (request.DrainageCount.HasValue) pond.DrainageCount = request.DrainageCount.Value;
                        if (request.PumpCapacity.HasValue) pond.PumpCapacity = request.PumpCapacity.Value;
                        pondRepos.Update(pond);
                        pondId = pond.Id;
                        await _unitOfWork.SaveChangesAsync(cancellationToken);
                    }
                    else
                    {
                        // Create
                        var validator = new CreateCommandValidator(localizer);
                        var validationResult = await validator.ValidateAsync(request, cancellationToken);
                        if (!validationResult.IsValid)
                        {
                            return CommandResult<Result>.Fail(validationResult.Errors.FirstOrDefault()?.ErrorMessage ?? "Error in validation");
                        }

                        var pond = new Domain.Entities.Pond
                        {
                            OwnerId = ownerId,
                            Name = request.Name!,
                            ImageUrl = request.ImageUrl,
                            Length = request.Length!.Value,
                            Width = request.Width!.Value,
                            Depth = request.Depth!.Value,
                            Volume = request.Volume!.Value,
                            DrainageCount = request.DrainageCount!.Value,
                            PumpCapacity = request.PumpCapacity!.Value
                        };
                        pondRepos.Add(pond);
                        await _unitOfWork.SaveChangesAsync(cancellationToken);
                        pondId = pond.Id;
                    }

                    return CommandResult<Result>.Success(new Result
                    {
                        Id = pondId,
                        Message = _localizer["Pond saved successfully"]
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
