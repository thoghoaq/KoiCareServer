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
            public CreateCommandValidator()
            {
                RuleFor(x => x.Name).NotNull().NotEmpty();
                RuleFor(x => x.Length).NotNull().NotEmpty().GreaterThan(0);
                RuleFor(x => x.Width).NotNull().NotEmpty().GreaterThan(0);
                RuleFor(x => x.Depth).NotNull().NotEmpty().GreaterThan(0);
                RuleFor(x => x.Volume).NotNull().NotEmpty().GreaterThan(0);
                RuleFor(x => x.DrainageCount).NotNull().NotEmpty().GreaterThan(0);
                RuleFor(x => x.PumpCapacity).NotNull().NotEmpty().GreaterThan(0);
            }
        }

        public class UpdateCommandValidator : AbstractValidator<Command>
        {
            public UpdateCommandValidator()
            {
                RuleFor(x => x.Id).NotEmpty();
                RuleFor(x => x.Name).NotEmpty();
                RuleFor(x => x.Length).NotEmpty().GreaterThan(0);
                RuleFor(x => x.Width).NotEmpty().GreaterThan(0);
                RuleFor(x => x.Depth).NotEmpty().GreaterThan(0);
                RuleFor(x => x.Volume).NotEmpty().GreaterThan(0);
                RuleFor(x => x.DrainageCount).NotEmpty().GreaterThan(0);
                RuleFor(x => x.PumpCapacity).NotEmpty().GreaterThan(0);
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
                        var validator = new UpdateCommandValidator();
                        var validationResult = await validator.ValidateAsync(request, cancellationToken);
                        if (!validationResult.IsValid)
                        {
                            return CommandResult<Result>.Fail(validationResult.Errors.FirstOrDefault()?.ErrorMessage ?? "Error in validation");
                        }

                        var pond = await pondRepos.Queryable().FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
                        if (pond == null)
                        {
                            return CommandResult<Result>.Fail(_localizer["Pond not found"]);
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
                        var validator = new CreateCommandValidator();
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
