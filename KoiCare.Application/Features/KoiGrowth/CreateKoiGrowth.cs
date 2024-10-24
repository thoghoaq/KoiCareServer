using KoiCare.Application.Abtractions.Database;
using KoiCare.Application.Abtractions.Localization;
using KoiCare.Application.Abtractions.LoggedUser;
using KoiCare.Application.Commons;
using KoiCare.Application.Features.Koifish;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Net;

namespace KoiCare.Application.Features.KoiGrowth
{
    public class CreateKoiGrowth
    {
        public class Command : IRequest<CommandResult<Result>>
        {
            public required int KoiIndividualId { get; set; }
            public required decimal Length { get; set; }
            public required decimal Weight { get; set; }
        }

        public class Result
        {
            public string? Message { get; set; }
        }

        public class Handler(
           IRepository<Domain.Entities.KoiGrowth> koiRepos,
           IAppLocalizer localizer,
           ILogger<CreateKoiGrowth> logger,
           ILoggedUser loggedUser,
           IUnitOfWork unitOfWork
        ) : BaseRequestHandler<Command, CommandResult<Result>>(localizer, logger, loggedUser, unitOfWork)
        {
            private readonly IRepository<Domain.Entities.KoiGrowth> _koiRepos = koiRepos;

            public override async Task<CommandResult<Result>> Handle(Command request, CancellationToken cancellationToken)
            {
                try
                {
                    var koifish = new Domain.Entities.KoiGrowth()
                    {
                        KoiIndividualId = request.KoiIndividualId,
                        Weight = request.Weight,
                        Length = request.Length,
                        MeasuredAt = DateTime.UtcNow,
                    };

                    await _koiRepos.AddAsync(koifish, cancellationToken);
                    await _unitOfWork.SaveChangesAsync(cancellationToken);

                    return CommandResult<Result>.Success(new Result
                    {
                        Message = _localizer["Koi growth create successfully"],
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
