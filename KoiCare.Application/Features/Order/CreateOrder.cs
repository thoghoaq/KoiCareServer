using KoiCare.Application.Abtractions.Database;
using KoiCare.Application.Commons;
using MediatR;
using Microsoft.Extensions.Logging;

namespace KoiCare.Application.Features.Order
{
    public class CreateOrder
    {
        public class Command : IRequest<CommandResult<bool>>
        {
            public int CustomerId { get; set; }
            public List<int> ProductIds { get; set; } = new();
            public decimal Total { get; set; }
        }

        public class Handler : IRequestHandler<Command, CommandResult<bool>>
        {
            private readonly IRepository<Domain.Entities.Order> _orderRepository;
            private readonly IUnitOfWork _unitOfWork;
            private readonly ILogger<Handler> _logger;

            public Handler(
                IRepository<Domain.Entities.Order> orderRepository,
                IUnitOfWork unitOfWork,
                ILogger<Handler> logger)
            {
                _orderRepository = orderRepository;
                _unitOfWork = unitOfWork;
                _logger = logger;
            }

            public async Task<CommandResult<bool>> Handle(Command request, CancellationToken cancellationToken)
            {
                var order = new Domain.Entities.Order
                {
                    CustomerId = request.CustomerId,
                    OrderDate = DateTime.UtcNow,
                    Total = request.Total,
                    CompletedAt = DateTime.UtcNow
                };

                _orderRepository.Add(order);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _logger.LogInformation("Order created successfully with ID: {OrderId}", order.Id);
                return CommandResult<bool>.Success(true);
            }
        }
    }
}
