using KoiCare.Application.Abtractions.Database;
using KoiCare.Application.Abtractions.Localization;
using KoiCare.Application.Abtractions.LoggedUser;
using KoiCare.Application.Commons;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Net;

public class CreateOrder
{
    public class Command : IRequest<CommandResult<Result>>
    {
        public required int CustomerId { get; set; }
        public required List<OrderDetailDto> OrderDetails { get; set; } = new();
        public required decimal Total { get; set; }
    }

    public class OrderDetailDto
    {
        public required int ProductId { get; set; }
        public required int Quantity { get; set; }
        public required decimal Price { get; set; }
    }

    public class Result
    {
        public string? Message { get; set; }
        public int? OrderId { get; set; }
        public string? OrderCode { get; set; }
    }

    public class Handler : BaseRequestHandler<Command, CommandResult<Result>>
    {
        private readonly IRepository<KoiCare.Domain.Entities.Order> _orderRepository;
        private readonly IRepository<KoiCare.Domain.Entities.OrderDetail> _orderDetailRepository;

        public Handler(
            IRepository<KoiCare.Domain.Entities.Order> orderRepository,
            IRepository<KoiCare.Domain.Entities.OrderDetail> orderDetailRepository,
            IAppLocalizer localizer,
            ILogger<CreateOrder> logger,
            ILoggedUser loggedUser,
            IUnitOfWork unitOfWork
        ) : base(localizer, logger, loggedUser, unitOfWork)
        {
            _orderRepository = orderRepository;
            _orderDetailRepository = orderDetailRepository;
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
                    IsCompleted = false,
                    CompletedAt = null // đơn hàng chưa hoàn thành
                };

                await _orderRepository.AddAsync(order, cancellationToken);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                // Tạo OrderDetail cho Order
                foreach (var detail in request.OrderDetails)
                {
                    var orderDetail = new KoiCare.Domain.Entities.OrderDetail
                    {
                        OrderId = order.Id,
                        ProductId = detail.ProductId,
                        Quantity = detail.Quantity,
                        Price = detail.Price
                    };
                    await _orderDetailRepository.AddAsync(orderDetail, cancellationToken);
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                // Tạo mã đơn hàng theo format ORDxxxxx
                string orderCode = $"ORD{order.Id:D5}";

                return CommandResult<Result>.Success(new Result
                {
                    Message = _localizer["Order created successfully"],
                    OrderId = order.Id,
                    OrderCode = orderCode
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

