using KoiCare.Application.Abtractions.Database;
using KoiCare.Application.Abtractions.Localization;
using KoiCare.Application.Abtractions.LoggedUser;
using KoiCare.Application.Commons;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace KoiCare.Application.Features.Order
{
    public class GetOrderDetails
    {
        public class OrderDetailsResult
        {
            public int Id { get; set; }
            public int OrderId { get; set; }
            public required string ProductName { get; set; }
            public int Quantity { get; set; }
            public decimal Price { get; set; }
        }

        public class Query : IRequest<CommandResult<Result>>
        {
            public int OrderId { get; set; }
        }

        public class Result
        {
            public List<OrderDetailsResult> OrderDetails { get; set; } = [];
        }

        public class Handler(
           IRepository<Domain.Entities.OrderDetail> orderDetailRepos,
           IAppLocalizer localizer,
           ILogger<GetOrderDetails> logger,
           ILoggedUser loggedUser,
           IUnitOfWork unitOfWork) : BaseRequestHandler<Query, CommandResult<Result>>(localizer, logger, loggedUser, unitOfWork)
        {
            public async override Task<CommandResult<Result>> Handle(Query request, CancellationToken cancellationToken)
            {
                // get orderdetails
                var orderdetails = await orderDetailRepos.Queryable()
                    .Include(x => x.Product)
                    .Include(x => x.Order)
                    .Where(x => x.OrderId == request.OrderId)
                    .Select(x => new OrderDetailsResult
                    {
                        Id = x.Id,
                        OrderId = x.OrderId,
                        ProductName = x.Product.Name,
                        Quantity = x.Quantity,
                        Price = x.Price,
                    }).ToListAsync(cancellationToken);

                return CommandResult<Result>.Success(new Result
                {
                    OrderDetails = orderdetails,
                });
            }
        }
    }


}
