using KoiCare.Application.Abtractions.Database;
using KoiCare.Application.Abtractions.Email;
using KoiCare.Application.Abtractions.Localization;
using KoiCare.Application.Abtractions.LoggedUser;
using KoiCare.Application.Abtractions.Payments;
using KoiCare.Application.Commons;
using KoiCare.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace KoiCare.Application.Features.Order
{
    public class CreateOrder
    {
        public class Command : IRequest<CommandResult<Result>>
        {
            public required string PhoneNumber { get; set; }
            public required string Address { get; set; }
            public required List<OrderDetailDto> OrderDetails { get; set; } = new();
        }

        public class OrderDetailDto
        {
            public required int ProductId { get; set; }
            public required int Quantity { get; set; }
        }

        public class Result
        {
            public string? Message { get; set; }
            public int? OrderId { get; set; }
            public string? OrderCode { get; set; }
            public string? PaymentUrl { get; set; }
        }

        public class Handler(
            IRepository<Domain.Entities.Order> _orderRepository,
            IRepository<Domain.Entities.Product> _productRepository,
            IEmailService _emailService,
            IAppLocalizer localizer,
            ILogger<CreateOrder> logger,
            ILoggedUser loggedUser,
            IUnitOfWork unitOfWork,
            IVnPayService vnPayService
        ) : BaseRequestHandler<Command, CommandResult<Result>>(localizer, logger, loggedUser, unitOfWork)
        {
            public override async Task<CommandResult<Result>> Handle(Command request, CancellationToken cancellationToken)
            {
                try
                {
                    // Validate products in the order details
                    var products = await _productRepository.Queryable()
                        .Where(p => request.OrderDetails.Select(d => d.ProductId).Contains(p.Id))
                        .ToListAsync(cancellationToken);

                    if (products.Count != request.OrderDetails.Count)
                    {
                        return CommandResult<Result>.Fail(HttpStatusCode.BadRequest, _localizer["Invalid product ID"]);
                    }

                    // Calculate total cost of the order
                    var total = products.Sum(p => p.Price * request.OrderDetails.First(d => d.ProductId == p.Id).Quantity);

                    // Create order entity
                    var order = new Domain.Entities.Order
                    {
                        CustomerId = _loggedUser.UserId,
                        OrderDate = DateTime.UtcNow,
                        Total = total,
                        PhoneNumber = request.PhoneNumber,
                        Address = request.Address,
                        IsCompleted = false,
                        CompletedAt = null,
                        OrderDetails = request.OrderDetails.Select(d => new OrderDetail
                        {
                            ProductId = d.ProductId,
                            Quantity = d.Quantity,
                            Price = products.First(p => p.Id == d.ProductId).Price
                        }).ToList()
                    };

                    // Save the order to the database
                    await _orderRepository.AddAsync(order, cancellationToken);
                    await _unitOfWork.SaveChangesAsync(cancellationToken);

                    // Generate order code in the format ORDxxxxx
                    string orderCode = $"ORD{order.Id:D5}";

                    // Generate VNPAY payment URL
                    string paymentUrl = vnPayService.GeneratePaymentUrl(order.Id, total);

                    // Send confirmation email to the customer
                    string subject = "Order Confirmation";
                    var parameters = new Dictionary<string, object>
                    {
                        { "CustomerName", _loggedUser.UserName },
                        { "OrderCode", orderCode },
                        { "OrderDate", order.OrderDate.ToString("yyyy-MM-dd") },
                        { "PhoneNumber", order.PhoneNumber },
                        { "Address", order.Address },
                        { "Total", total.ToString("C") },
                        { "OrderDetails", GenerateOrderDetailsHtml(order.OrderDetails.ToList(), products) }
                    };
                    await _emailService.SendEmailAsync(_loggedUser.Email, subject, Domain.Enums.EEmailTemplate.CustomerOrder, parameters);

                    // Return success with order details and payment URL
                    return CommandResult<Result>.Success(new Result
                    {
                        Message = _localizer["Order created successfully"],
                        OrderId = order.Id,
                        OrderCode = orderCode,
                        PaymentUrl = paymentUrl
                    });
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "CreateOrderError");
                    return CommandResult<Result>.Fail(HttpStatusCode.InternalServerError, ex.Message);
                }
            }

            // Helper method to format order details in HTML
            private static string GenerateOrderDetailsHtml(List<OrderDetail> orderDetails, List<Domain.Entities.Product> products)
            {
                CultureInfo vietnamCulture = new CultureInfo("vi-VN");
                var sb = new StringBuilder();
                sb.Append("<ul>");
                foreach (var detail in orderDetails)
                {
                    sb.AppendFormat($"<li>Product ID: {detail.ProductId}, Product Name: {products.First(x => x.Id == detail.ProductId).Name}, Quantity: {detail.Quantity}, Price: {detail.Price.ToString("N0", vietnamCulture)} VND</li>");
                }
                sb.Append("</ul>");
                return sb.ToString();
            }
        }
    }
}
