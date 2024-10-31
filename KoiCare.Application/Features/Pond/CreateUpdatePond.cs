using FluentValidation;
using KoiCare.Application.Abtractions.Database;
using KoiCare.Application.Abtractions.Localization;
using KoiCare.Application.Abtractions.LoggedUser;
using KoiCare.Application.Commons;
using KoiCare.Domain.Enums;
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
            public int? KoiGroupId { get; set; }
            public EAgeRange? AgeRange { get; set; }
            public EGender? Gender { get; set; }
            public virtual SaltRequirement? SaltRequirement { get; set; }
            public virtual WaterParameter? WaterParameter { get; set; }
        }

        public record SaltRequirement
        {
            public decimal? RequiredAmount { get; set; }
        }

        public record WaterParameter
        {
            public decimal? Temperature { get; set; }
            public decimal? Salinity { get; set; }
            public decimal? Ph { get; set; }
            public decimal? Oxygen { get; set; }
            public decimal? NO2 { get; set; }
            public decimal? NO3 { get; set; }
            public decimal? PO4 { get; set; }
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
                RuleFor(x => x.Length)
                    .GreaterThan(0).WithMessage(localizer["Length is greater than 0"]);
                RuleFor(x => x.Width)
                    .GreaterThan(0).WithMessage(localizer["Width is greater than 0"]);
                RuleFor(x => x.Depth)
                    .GreaterThan(0).WithMessage(localizer["Depth is greater than 0"]);
                RuleFor(x => x.Volume)
                    .GreaterThan(0).WithMessage(localizer["Volume is greater than 0"]);
                RuleFor(x => x.DrainageCount)
                    .GreaterThan(0).WithMessage(localizer["Drainage count is greater than 0"]);
                RuleFor(x => x.PumpCapacity)
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
            IRepository<Domain.Entities.SaltRequirement> saltRequirementRepos,
            IRepository<Domain.Entities.WaterParameter> waterParameterRepos,
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
                        var validator = new UpdateCommandValidator(_localizer);
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
                        if (pond.OwnerId != ownerId && !_loggedUser.IsAdmin)
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
                        if (request.AgeRange.HasValue) pond.AgeRange = request.AgeRange.Value;
                        if (request.KoiGroupId.HasValue) pond.KoiGroupId = request.KoiGroupId.Value;
                        if (request.Gender.HasValue) pond.Gender = request.Gender.Value;
                        pondRepos.Update(pond);
                        pondId = pond.Id;
                        await _unitOfWork.SaveChangesAsync(cancellationToken);
                    }
                    else
                    {
                        // Create
                        var validator = new CreateCommandValidator(_localizer);
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
                            PumpCapacity = request.PumpCapacity!.Value,
                            AgeRange = request.AgeRange,
                            KoiGroupId = request.KoiGroupId,
                            Gender = request.Gender
                        };
                        pondRepos.Add(pond);
                        await _unitOfWork.SaveChangesAsync(cancellationToken);
                        pondId = pond.Id;
                    }

                    if (request.SaltRequirement != null)
                    {
                        if (!request.SaltRequirement.RequiredAmount.HasValue)
                        {
                            return CommandResult<Result>.Fail(_localizer["Required amount is required"]);
                        }
                        var saltRequirement = await saltRequirementRepos.Queryable().FirstOrDefaultAsync(x => x.PondId == pondId, cancellationToken);
                        if (saltRequirement != null)
                        {
                            saltRequirement.RequiredAmount = (decimal)request.SaltRequirement.RequiredAmount;
                            saltRequirementRepos.Update(saltRequirement);
                        }
                        else
                        {
                            saltRequirement = new Domain.Entities.SaltRequirement
                            {
                                PondId = pondId,
                                RequiredAmount = (decimal)request.SaltRequirement.RequiredAmount
                            };
                            saltRequirementRepos.Add(saltRequirement);
                        }
                        await _unitOfWork.SaveChangesAsync(cancellationToken);
                    }

                    if (request.WaterParameter != null)
                    {
                        waterParameterRepos.Add(new Domain.Entities.WaterParameter
                        {
                            PondId = pondId,
                            Temperature = request.WaterParameter.Temperature,
                            Salinity = request.WaterParameter.Salinity,
                            Ph = request.WaterParameter.Ph,
                            Oxygen = request.WaterParameter.Oxygen,
                            NO2 = request.WaterParameter.NO2,
                            NO3 = request.WaterParameter.NO3,
                            PO4 = request.WaterParameter.PO4,
                            MeasuredAt = DateTime.UtcNow
                        });
                        await _unitOfWork.SaveChangesAsync(cancellationToken);
                    }

                    return CommandResult<Result>.Success(new Result
                    {
                        Id = pondId,
                        Message = _localizer["Pond saved successfully"]
                    });
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "CreateUpdatePondError");
                    return CommandResult<Result>.Fail(HttpStatusCode.InternalServerError, ex.Message);
                }
            }
        }
    }
}
