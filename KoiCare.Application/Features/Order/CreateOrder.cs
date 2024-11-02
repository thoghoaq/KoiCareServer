using KoiCare.Application.Abtractions.Database;
using KoiCare.Application.Abtractions.Email;
using KoiCare.Application.Abtractions.Localization;
using KoiCare.Application.Abtractions.LoggedUser;
using KoiCare.Application.Commons;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.IO;
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

                // Get customer email and name
                var customer = await GetCustomerByIdAsync(request.CustomerId, cancellationToken);
                var customerEmail = customer?.Email ?? throw new InvalidOperationException("Customer email not found");
                var customerName = customer?.Username ?? "Customer";

                // Set email subject
                string subject = "Order Confirmation";

                // Load the email template and replace placeholders
                string templatePath = "C:\\School\\swp\\KoiCareServer\\KoiCare.Infrastructure\\Dependencies\\Email\\Templates\\OrderConfirmationTemplate.html";
                string htmlTemplate = await File.ReadAllTextAsync(templatePath);
                string htmlMessage = htmlTemplate
                    .Replace("{{CustomerName}}", customerName)
                    .Replace("{{OrderCode}}", orderCode)
                    .Replace("{{OrderDate}}", order.OrderDate.ToString("yyyy-MM-dd"))
                    .Replace("{{OrderDetails}}", GenerateOrderDetailsHtml(request.OrderDetails));

                // Send the email using the HTML template
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

        // Helper method to get customer by ID
        private async Task<KoiCare.Domain.Entities.User?> GetCustomerByIdAsync(int customerId, CancellationToken cancellationToken)
        {
            return await _customerRepository.Queryable()
                .Where(c => c.Id == customerId)
                .FirstOrDefaultAsync(cancellationToken);
        }

        // Helper method to format order details in HTML
        private string GenerateOrderDetailsHtml(List<OrderDetailDto> orderDetails)
        {
            var html = "<ul>";
            foreach (var detail in orderDetails)
            {
                html += $"<li>Product ID: {detail.ProductId}, Quantity: {detail.Quantity}, Price: {detail.Price:C}</li>";
            }
            html += "</ul>";
            return html;
        }
    }
}
