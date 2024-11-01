using KoiCare.Application.Abtractions.Database;
using KoiCare.Application.Abtractions.Localization;
using KoiCare.Application.Abtractions.LoggedUser;
using KoiCare.Application.Commons;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace KoiCare.Application.Features.Order
{
    public class CreateOrderDetail
    {
        public class Command : IRequest<CommandResult<bool>>
        {
            public int OrderId { get; set; }
            public int ProductId { get; set; }
            public int Quantity { get; set; }
            public decimal Price { get; set; }
        }

        public class Handler(
            IRepository<Domain.Entities.OrderDetail> orderDetailRepository,
            IRepository<Domain.Entities.Order> orderRepository,
            IRepository<Domain.Entities.Product> productRepository,
            IAppLocalizer localizer,
            ILogger<CreateOrderDetail> logger,
            ILoggedUser loggedUser,
            IUnitOfWork unitOfWork
        ) : BaseRequestHandler<Command, CommandResult<bool>>(localizer, logger, loggedUser, unitOfWork)
        {
            public override async Task<CommandResult<bool>> Handle(Command request, CancellationToken cancellationToken)
            {
                // kiểm tra order
                var order = await orderRepository.Queryable()
                    .FirstOrDefaultAsync(x => x.Id == request.OrderId, cancellationToken);

                if (order == null)
                {
                    _logger.LogWarning("Order with ID {OrderId} not found.", request.OrderId);
                    return CommandResult<bool>.Fail(HttpStatusCode.NotFound, _localizer["Order not found"]);
                }

                // kiểm tra product
                var product = await productRepository.Queryable()
                    .FirstOrDefaultAsync(p => p.Id == request.ProductId, cancellationToken);

                if (product == null)
                {
                    _logger.LogWarning("Product with ID {ProductId} not found.", request.ProductId);
                    return CommandResult<bool>.Fail(HttpStatusCode.NotFound, _localizer["Product not found"]);
                }

                // Create new OrderDetail
                var orderDetail = new Domain.Entities.OrderDetail
                {
                    OrderId = request.OrderId,
                    ProductId = request.ProductId,
                    Quantity = request.Quantity,
                    Price = request.Price
                };

                await orderDetailRepository.AddAsync(orderDetail, cancellationToken);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _logger.LogInformation("OrderDetail created successfully for Order ID {OrderId}.", request.OrderId);
                return CommandResult<bool>.Success(true);
            }
        }
    }
}
