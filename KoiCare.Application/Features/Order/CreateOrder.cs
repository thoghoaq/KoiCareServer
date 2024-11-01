using KoiCare.Application.Abtractions.Database;
using KoiCare.Application.Abtractions.Localization;
using KoiCare.Application.Abtractions.LoggedUser;
using KoiCare.Application.Commons;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Net;

public class CreateOrder
{
    public class Command : IRequest<CommandResult<Result>>
    {
        public required int CustomerId { get; set; }
        public required List<int> ProductIds { get; set; }
        public required decimal Total { get; set; }
    }

    public class Result
    {
        public string? Message { get; set; }
        public int? OrderId { get; set; }
    }

    public class Handler : BaseRequestHandler<Command, CommandResult<Result>>
    {
        private readonly IRepository<KoiCare.Domain.Entities.Order> _orderRepository;

        public Handler(
            IRepository<KoiCare.Domain.Entities.Order> orderRepository,
            IAppLocalizer localizer,
            ILogger<CreateOrder> logger,
            ILoggedUser loggedUser,
            IUnitOfWork unitOfWork
        ) : base(localizer, logger, loggedUser, unitOfWork)
        {
            _orderRepository = orderRepository;
        }

        public override async Task<CommandResult<Result>> Handle(Command request, CancellationToken cancellationToken)
        {
            try
            {
                var order = new KoiCare.Domain.Entities.Order
                {
                    CustomerId = request.CustomerId,
                    OrderDate = DateTime.UtcNow,
                    Total = request.Total,
                    CompletedAt = null // đơn hàng chưa hoàn thành
                };

                await _orderRepository.AddAsync(order, cancellationToken);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return CommandResult<Result>.Success(new Result
                {
                    Message = _localizer["Order created successfully"],
                    OrderId = order.Id
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "CreateOrderError");
                return CommandResult<Result>.Fail(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
