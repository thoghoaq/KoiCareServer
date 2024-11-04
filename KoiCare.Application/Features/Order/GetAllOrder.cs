using KoiCare.Application.Abtractions.Database;
using KoiCare.Application.Abtractions.Localization;
using KoiCare.Application.Abtractions.LoggedUser;
using KoiCare.Application.Commons;
using KoiCare.Domain.Enums;
using MediatR;
using Microsoft.Extensions.Logging;

namespace KoiCare.Application.Features.Order
{
    public class GetAllOrder
    {
        public class Query : IRequest<CommandResult<Result>>
        {
            public int? PageNumber { get; set; }
            public int? PageSize { get; set; }
            public string? Search { get; set; }
        }

        public class OrderResult
        {
            public int Id { get; set; }
            public required string OrderCode { get; set; }
            public required string CustomerName { get; set; }
            public DateTime OrderDate { get; set; }
            public required string PhoneNumber { get; set; }
            public required string Address { get; set; }
            public decimal Total { get; set; }
            public bool IsCompleted { get; set; }
            public DateTime? CompletedAt { get; set; }
        }

        public class Result
        {
            public List<OrderResult> Orders { get; set; } = [];
        }

        public class Handler(
            IAppLocalizer localizer,
            ILogger<GetAllOrder> logger,
            ILoggedUser loggedUser,
            IUnitOfWork unitOfWork,
            IRepository<Domain.Entities.Order> orderRepos
            ) : BaseRequestHandler<Query, CommandResult<Result>>(localizer, logger, loggedUser, unitOfWork)
        {
            public override Task<CommandResult<Result>> Handle(Query request, CancellationToken cancellationToken)
            {
                var query = orderRepos.Queryable()
                    // get orders is owned by the logged user, if admin get all orders
                    .Where(x => _loggedUser.RoleId == (int)ERole.Admin || x.CustomerId == _loggedUser.UserId)
                    .Where(x => request.Search == null || x.Customer.Username.Contains(request.Search!))
                    .Select(x => new OrderResult
                    {
                        Id = x.Id,
                        OrderCode = $"ORD{x.Id:D5}",
                        CustomerName = x.Customer.Username,
                        OrderDate = x.OrderDate,
                        Address = x.Address,
                        PhoneNumber = x.PhoneNumber,
                        Total = x.Total,
                        IsCompleted = x.IsCompleted,
                        CompletedAt = x.CompletedAt,
                    });

                // Pagination
                if (request.PageNumber.HasValue && request.PageSize.HasValue)
                {
                    query = query.Skip((request.PageNumber.Value - 1) * request.PageSize.Value).Take(request.PageSize.Value);
                }

                // Return the result
                return Task.FromResult(CommandResult<Result>.Success(new Result
                {
                    Orders = query.ToList()
                }));
            }
        }
    }
}
