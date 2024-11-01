using KoiCare.Application.Abtractions.Database;
using KoiCare.Application.Abtractions.Localization;
using KoiCare.Application.Abtractions.LoggedUser;
using KoiCare.Application.Commons;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Net;

namespace KoiCare.Application.Features.Order
{
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

        public class Handler(
            IRepository<Domain.Entities.Order> orderRepository,
            IAppLocalizer localizer,
            ILogger<CreateOrder> logger,
            ILoggedUser loggedUser,
            IUnitOfWork unitOfWork
        ) : BaseRequestHandler<Command, CommandResult<Result>>(localizer, logger, loggedUser, unitOfWork)
        {
            private readonly IRepository<Domain.Entities.Order> _orderRepository = orderRepository;

            public override async Task<CommandResult<Result>> Handle(Command request, CancellationToken cancellationToken)
            {
                try
                {
                    // Tạo đơn hàng mới
                    var order = new Domain.Entities.Order
                    {
                        CustomerId = request.CustomerId,
                        OrderDate = DateTime.UtcNow,
                        Total = request.Total,
                        CompletedAt = DateTime.UtcNow
                    };

                    // Thêm đơn hàng vào cơ sở dữ liệu
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
}
