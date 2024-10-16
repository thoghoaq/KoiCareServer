using KoiCare.Application.Abtractions.Database;
using KoiCare.Application.Abtractions.Localization;
using KoiCare.Application.Abtractions.LoggedUser;
using KoiCare.Application.Commons;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace KoiCare.Application.Features.Product
{
    public class GetProductByID
    {
        public class Result
        {
            public int Id { get; set; }
            public string Name { get; set; } = string.Empty;
            public string Description { get; set; } = string.Empty;
            public decimal Price { get; set; } = decimal.Zero;
            public string ImageUrl { get; set; }
            public string CategoryName { get; set; }
        }

        public class Query : IRequest<CommandResult<Result>>
        {
            public int Id { get; set; }
        }

        public class Handler(
            IAppLocalizer localizer,
            ILogger<GetProductByID> logger,
            ILoggedUser loggedUser,
            IUnitOfWork unitOfWork,
            IRepository<Domain.Entities.Product> productRepos
            ) : BaseRequestHandler<Query, CommandResult<Result>>(localizer, logger, loggedUser, unitOfWork)
        {
            public override async Task<CommandResult<Result>> Handle(Query request, CancellationToken cancellationToken)
            {
                var product = await productRepos.Queryable()
                    .Include(x => x.Category)
                    .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

                if (product == null)
                {
                    return CommandResult<Result>.Fail("Product does not exist");
                }

                var result = new Result
                {
                    Id = product.Id,
                    Name = product.Name,
                    Description = product.Description,
                    Price = product.Price,
                    ImageUrl = product.ImageUrl,
                    CategoryName = product.Category.Name,
                };

                return CommandResult<Result>.Success(result);
            }
        }
    }
}
