using KoiCare.Application.Abtractions.Database;
using KoiCare.Application.Abtractions.Localization;
using KoiCare.Application.Abtractions.LoggedUser;
using KoiCare.Application.Commons;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace KoiCare.Application.Features.Product
{
    public class GetAllProduct
    {
        public record ProductResult
        {
            public int Id { get; set; }
            public required string Name { get; set; }
            public string? Description { get; set; }
            public decimal Price { get; set; }
            public string? ImageUrl { get; set; }
            public required string Category { get; set; }
        }

        public class Result
        {
            public List<ProductResult> Products { get; set; } = new List<ProductResult>();
        }
        public class Query : IRequest<CommandResult<Result>>
        {
            public int? PageNumber { get; set; }
            public int? PageSize { get; set; }
            public string? Search { get; set; }
            public int? CategoryId { get; set; }
        }

        public class Handler(
            IAppLocalizer localizer,
            ILogger<GetAllProduct> logger,
            ILoggedUser loggedUser,
            IUnitOfWork unitOfWork,
            IRepository<Domain.Entities.Product> productRepos
            ) : BaseRequestHandler<Query, CommandResult<Result>>(localizer, logger, loggedUser, unitOfWork)
        {
            public override Task<CommandResult<Result>> Handle(Query request, CancellationToken cancellationToken)
            {
                // Query for products
                var query = productRepos.Queryable()
                    .Include(x => x.Category) // Assuming there's an Category relationship
                    .Where(x => request.Search == null || x.Name.Contains(request.Search!) || x.Description.Contains(request.Search!))
                    .Where(x => request.CategoryId == null || x.CategoryId.Equals(request.CategoryId))
                    .Select(x => new ProductResult
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Description = x.Description,
                        Price = x.Price,
                        ImageUrl = x.ImageUrl,
                        Category = x.Category.Name 
                    });

                // Pagination
                if (request.PageNumber.HasValue && request.PageSize.HasValue)
                {
                    query = query.Skip((request.PageNumber.Value - 1) * request.PageSize.Value).Take(request.PageSize.Value);
                }

                // Return the result
                return Task.FromResult(CommandResult<Result>.Success(new Result
                {
                    Products = query.ToList()
                }));
            }
        }
    }
}
