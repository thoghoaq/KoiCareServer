
using KoiCare.Application.Abtractions.Database;
using KoiCare.Application.Abtractions.Localization;
using KoiCare.Application.Abtractions.LoggedUser;
using KoiCare.Application.Commons;
using KoiCare.Application.Features.Koifish;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Net;

namespace KoiCare.Application.Features.KoiGrowth
{
    public class UpdateKoiGrowth
    {
        public class Result
        {
            public string? Message { get; set; }
        }

        public class Command : IRequest<CommandResult<Result>>
        {
            public int Id { get; set; }
            public int? KoiIndividualId { get; set; }
            public decimal? Length { get; set; }
            public decimal? Weight { get; set; }
        }

        public class Handler(
            IAppLocalizer localizer,
            ILogger<UpdateKoiGrowth> logger,
            ILoggedUser loggedUser,
            IUnitOfWork unitOfWork,
            IRepository<Domain.Entities.KoiGrowth> koiRepos
            ) : BaseRequestHandler<Command, CommandResult<Result>>(localizer, logger, loggedUser, unitOfWork)
        {
            public override async Task<CommandResult<Result>> Handle(Command request, CancellationToken cancellationToken)
            {
                try
                {
                    //Tìm KoiGrowth trong cơ sở dữ liệu
                    var koifish = await koiRepos.Queryable().FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
                    if (koifish == null)
                    {
                        return CommandResult<Result>.Fail(_localizer["Thông tin tăng trưởng không tồn tại"]);
                    }
                    //Update KoiGrowth
                    if (request.KoiIndividualId != null) koifish.KoiIndividualId = request.KoiIndividualId.Value;
                    if (request.Length.HasValue) koifish.Length = request.Length.Value;
                    if (request.Weight.HasValue) koifish.Weight = request.Weight.Value;
                    koiRepos.Update(koifish);
                    await _unitOfWork.SaveChangesAsync(cancellationToken);

                    return CommandResult<Result>.Success(new Result
                    {
                        Message = _localizer["Cập nhật thông tin cá koi thành công"]
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
