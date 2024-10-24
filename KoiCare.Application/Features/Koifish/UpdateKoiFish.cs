using KoiCare.Application.Abtractions.Database;
using KoiCare.Application.Abtractions.Localization;
using KoiCare.Application.Abtractions.LoggedUser;
using KoiCare.Application.Commons;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Net;

namespace KoiCare.Application.Features.Koifish
{
    public class UpdateKoiFish
    {
        public class Result
        {
            public string? Message { get; set; }
        }

        public class Command : IRequest<CommandResult<Result>>
        {
            public required int Id { get; set; }
            public string? Name { get; set; }
            public int? KoiTypeId { get; set; }
            public int? PondId { get; set; }
            public string? ImageUrl { get; set; }
            public decimal? Age { get; set; }
            public decimal? Length { get; set; }
            public decimal? Weight { get; set; }
            public int? Gender { get; set; }
            public string? Origin { get; set; }
            public int? Shape { get; set; }
            public string? Breed { get; set; }
        }

        public class Handler(
            IAppLocalizer localizer,
            ILogger<UpdateKoiFish> logger,
            ILoggedUser loggedUser,
            IUnitOfWork unitOfWork,
            IRepository<Domain.Entities.KoiIndividual> koiRepos
            ) : BaseRequestHandler<Command, CommandResult<Result>>(localizer, logger, loggedUser, unitOfWork)
        {
            public override async Task<CommandResult<Result>> Handle(Command request, CancellationToken cancellationToken)
            {
                try
                {
                    //Tìm KoiIndividual trong cơ sở dữ liệu
                    var koifish = await koiRepos.Queryable().FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
                    if (koifish == null)
                    {
                        return CommandResult<Result>.Fail(_localizer["Cá koi không tồn tại"]);
                    }
                    //Update KoiFish
                    if (request.Name != null) koifish.Name = request.Name;
                    if (request.KoiTypeId != null) koifish.KoiTypeId = request.KoiTypeId.Value;
                    if (request.PondId != null) koifish.PondId = request.PondId.Value;
                    if (request.ImageUrl != null) koifish.ImageUrl = request.ImageUrl;
                    if (request.Age.HasValue) koifish.Age = request.Age.Value;
                    if (request.Length.HasValue) koifish.Length = request.Length.Value;
                    if (request.Weight.HasValue) koifish.Weight = request.Weight.Value;
                    if (request.Gender != null) koifish.Gender = request.Gender.Value;
                    if (request.Origin != null) koifish.Origin = request.Origin;
                    if (request.Shape.HasValue) koifish.Shape = request.Shape.Value;
                    if (request.Breed != null) koifish.Breed = request.Breed;

                    koiRepos.Update(koifish);
                    await _unitOfWork.SaveChangesAsync(cancellationToken);

                    return CommandResult<Result>.Success(new Result
                    {
                        Message = _localizer["Cập nhật cá koi thành công"]
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