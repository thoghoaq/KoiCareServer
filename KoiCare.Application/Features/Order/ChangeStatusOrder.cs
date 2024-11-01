using System.Net;
using MediatR;
using KoiCare.Application.Abtractions.Database;
using KoiCare.Application.Abtractions.Localization;
using KoiCare.Application.Abtractions.LoggedUser;
using KoiCare.Application.Commons;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

namespace KoiCare.Application.Features.Order
{
    public class ChangeStatusOrder
    {
        public class Command : IRequest<CommandResult<Result>>
        {
            public int OrderId { get; set; }
            public bool IsCompleted { get; set; }
        }

        public class Result
        {
            public string? Message { get; set; }
        }

        public class Handler(
            IRepository<Domain.Entities.Order> orderRepository,
            IAppLocalizer localizer,
            ILogger<ChangeStatusOrder> logger,
            ILoggedUser loggedUser,
            IUnitOfWork unitOfWork
        ) : BaseRequestHandler<Command, CommandResult<Result>>(localizer, logger, loggedUser, unitOfWork)
        {
            private readonly IRepository<Domain.Entities.Order> _orderRepository = orderRepository;

            public override async Task<CommandResult<Result>> Handle(Command request, CancellationToken cancellationToken)
            {
                // Tìm đơn hàng theo ID
                var order = await _orderRepository.Queryable()
                    .FirstOrDefaultAsync(o => o.Id == request.OrderId, cancellationToken);

                if (order == null)
                {
                    _logger.LogWarning("Order with ID {OrderId} not found.", request.OrderId);
                    return CommandResult<Result>.Fail(HttpStatusCode.NotFound, _localizer["Order not found"]);
                }

                // Cập nhật trạng thái hoàn thành của đơn hàng
                order.IsCompleted = request.IsCompleted;
                order.CompletedAt = request.IsCompleted ? DateTime.UtcNow : (DateTime?)null;

                _orderRepository.Update(order);
                var saveResult = await _unitOfWork.SaveChangesAsync(cancellationToken);

                if (saveResult <= 0)
                {
                    _logger.LogError("Failed to update order status for Order ID {OrderId}.", request.OrderId);
                    return CommandResult<Result>.Fail(HttpStatusCode.InternalServerError, _localizer["Failed to update order status"]);
                }

                _logger.LogInformation("Order status updated successfully for Order ID {OrderId}.", request.OrderId);
                return CommandResult<Result>.Success(new Result
                {
                    Message = _localizer["Order status updated successfully"]
                });
            }
        }
    }
}
