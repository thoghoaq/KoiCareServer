using KoiCare.Application.Abtractions.Database;
using KoiCare.Application.Abtractions.Localization;
using KoiCare.Application.Abtractions.LoggedUser;
using MediatR;
using Microsoft.Extensions.Logging;

namespace KoiCare.Application.Commons
{
    public abstract class BaseRequestHandler<TCommand, TResult>(
        IAppLocalizer localizer,
        ILogger logger,
        ILoggedUser loggedUser,
        IUnitOfWork unitOfWork) : IRequestHandler<TCommand, TResult> where TCommand : IRequest<TResult>
    {
        protected readonly IAppLocalizer _localizer = localizer;
        protected readonly ILogger _logger = logger;
        protected readonly ILoggedUser _loggedUser = loggedUser;
        protected readonly IUnitOfWork _unitOfWork = unitOfWork;

        public abstract Task<TResult> Handle(TCommand request, CancellationToken cancellationToken);
    }
}
