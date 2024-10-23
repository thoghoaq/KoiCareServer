
using KoiCare.Application.Abtractions.Database;
using KoiCare.Application.Abtractions.Localization;
using KoiCare.Application.Abtractions.LoggedUser;
using KoiCare.Application.Commons;
using KoiCare.Application.Features.Blog;
using KoiCare.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Net;

namespace KoiCare.Application.Features.KoiGrowth
{
    public class DeleteKoiGrowth
    {
        public class Command : IRequest<CommandResult<Result>>
        {
            public required int Id { get; set; }
        }

        public class Result
        {
            public string? Message { get; set; }
        }

        public class Handler(
            IAppLocalizer localizer,
            ILogger<DeleteKoiGrowth> logger,
            ILoggedUser loggedUser,
            IUnitOfWork unitOfWork,
            IRepository<Domain.Entities.KoiGrowth> koiRepos
            ) : BaseRequestHandler<Command, CommandResult<Result>>(localizer, logger, loggedUser, unitOfWork)
        {
            public override async Task<CommandResult<Result>> Handle(Command request, CancellationToken cancellationToken)
            {
                try
                {
                    var koigrowth = await koiRepos.Queryable().FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
                    if (koigrowth == null)
                    {
                        return CommandResult<Result>.Fail(_localizer["Thông tin tăng trưởng cá không tồn tại"]);
                    }

                    koiRepos.Remove(koigrowth);
                    await _unitOfWork.SaveChangesAsync(cancellationToken);
                    return CommandResult<Result>.Success(new Result
                    {
                        Message = _localizer["Xóa thông tin thành công"]
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
