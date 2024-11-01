using KoiCare.Application.Abtractions.Database;
using KoiCare.Application.Abtractions.Email;
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
        private readonly IRepository<KoiCare.Domain.Entities.User> _customerRepository; 
        private readonly IEmailService _emailService;

        public Handler(
            IRepository<KoiCare.Domain.Entities.Order> orderRepository,
            IRepository<KoiCare.Domain.Entities.OrderDetail> orderDetailRepository,
            IRepository<KoiCare.Domain.Entities.User> customerRepository, 
            IEmailService emailService,
            IAppLocalizer localizer,
            ILogger<CreateOrder> logger,
            ILoggedUser loggedUser,
            IUnitOfWork unitOfWork
        ) : base(localizer, logger, loggedUser, unitOfWork)
        {
            _orderRepository = orderRepository;
            _orderDetailRepository = orderDetailRepository;
            _customerRepository = customerRepository;
            _emailService = emailService;
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
                    CompletedAt = null 
                };

                await _orderRepository.AddAsync(order, cancellationToken);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                // Create OrderDetail for Order
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

                // Generate order code in the format ORDxxxxx
                string orderCode = $"ORD{order.Id:D5}";

                // Get customer email
                var customerEmail = await GetCustomerEmailByIdAsync(request.CustomerId, cancellationToken);

                // Prepare email subject and body
                string subject = "Order Confirmation";
                string htmlMessage = $"<h1>Your Order has been created successfully!</h1>" +
                                     $"<p>Order Code: {orderCode}</p>" +
                                     $"<p>Thank you for your order!</p>";

                // Send email notification
                await _emailService.SendEmailAsync(customerEmail, subject, htmlMessage);

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

        // Helper method to get customer email
        private async Task<string> GetCustomerEmailByIdAsync(int customerId, CancellationToken cancellationToken)
        {
            var customer = await _customerRepository.Queryable()
                .Where(c => c.Id == customerId)
                .Select(c => c.Email)
                .FirstOrDefaultAsync(cancellationToken);

            return customer ?? throw new InvalidOperationException("Customer not found");
        }
    }
}
