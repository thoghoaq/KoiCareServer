using KoiCare.Application.Abtractions.Database;
using KoiCare.Application.Abtractions.Localization;
using KoiCare.Application.Abtractions.LoggedUser;
using KoiCare.Application.Abtractions.Storage;
using KoiCare.Application.Commons;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net;

namespace KoiCare.Application.Features.Image
{
    public class UploadImage
    {
        public class Result
        {
            public string? ImageUrl { get; set; }
        }

        public class Command : IRequest<CommandResult<Result>>
        {
            public required IFormFile File { get; set; }
        }

        public class Handler(
            IAppLocalizer localizer,
            ILogger<UploadImage> logger,
            ILoggedUser loggedUser,
            IUnitOfWork unitOfWork,
            IStorageService _storageService
            ) : BaseRequestHandler<Command, CommandResult<Result>>(localizer, logger, loggedUser, unitOfWork)
        {
            public override async Task<CommandResult<Result>> Handle(Command request, CancellationToken cancellationToken)
            {
                try
                {
                    var imageUrl = await _storageService.UploadImageAsync(request.File, "KoiCare");
                    return CommandResult<Result>.Success(new Result { ImageUrl = imageUrl });
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
