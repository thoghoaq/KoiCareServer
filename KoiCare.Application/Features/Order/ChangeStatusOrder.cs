using System.Net;
using MediatR;
using KoiCare.Application.Abtractions.Database;
using KoiCare.Application.Commons;
using KoiCare.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace KoiCare.Application.Features.Order
{
    public class ChangeStatusOrder
    {
        public class Command : IRequest<CommandResult<bool>>
        {
            public int OrderId { get; set; }
            public bool IsCompleted { get; set; }
        }

        public class Handler : IRequestHandler<Command, CommandResult<bool>>
        {
            private readonly IRepository<Domain.Entities.Order> _orderRepository;
            private readonly IUnitOfWork _unitOfWork;
            private readonly ILogger<Handler> _logger;

            public Handler(IUnitOfWork unitOfWork, ILogger<Handler> logger)
            {
                _unitOfWork = unitOfWork;
                _orderRepository = unitOfWork.Repository<Domain.Entities.Order>();
                _logger = logger;
            }

            public async Task<CommandResult<bool>> Handle(Command request, CancellationToken cancellationToken)
            {
                var order = _orderRepository.Find(request.OrderId);

                if (order == null)
                {
                    _logger.LogWarning($"Order with ID {request.OrderId} not found.");
                    return CommandResult<bool>.Fail(HttpStatusCode.NotFound, "Order not found");
                }

                order.IsCompleted = request.IsCompleted;
                _orderRepository.Update(order);

                var saveResult = await _unitOfWork.SaveChangesAsync(cancellationToken);
                if (saveResult <= 0)
                {
                    _logger.LogError($"Failed to update order with ID {request.OrderId}.");
                    return CommandResult<bool>.Fail(HttpStatusCode.InternalServerError, "Failed to update order status");
                }

                _logger.LogInformation($"Order status updated successfully for order ID {request.OrderId}.");
                return CommandResult<bool>.Success(true);
            }
        }
    }
}
