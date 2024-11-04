using KoiCare.Application.Abtractions.Database;
using KoiCare.Application.Abtractions.Localization;
using KoiCare.Application.Abtractions.LoggedUser;
using KoiCare.Application.Commons;
using MediatR;
using Microsoft.Extensions.Logging;

namespace KoiCare.Application.Features.Category
{
    public class GetAllCategory
    {
        public record CategoryResult
        {
            public required int Id { get; set; }
            public required string Name { get; set; }
            public string? ImageUrl { get; set; }

        }

        public class Result
        {
            public List<CategoryResult> Results { get; set; } = new List<CategoryResult>();
        }

        public class Query : IRequest<CommandResult<Result>>
        {
            public int? PageNumber { get; set; }
            public int? PageSize { get; set; }
            public string? Search { get; set; }
        }

        public class Handler(
            IAppLocalizer localizer,
            ILogger<GetAllCategory> logger,
            ILoggedUser loggedUser,
            IUnitOfWork unitOfWork,
            IRepository<Domain.Entities.Category> cateRepos
            ) : BaseRequestHandler<Query, CommandResult<Result>>(localizer, logger, loggedUser, unitOfWork)
        {
            public override Task<CommandResult<Result>> Handle(Query request, CancellationToken cancellationToken)
            {
                var query = cateRepos.Queryable()
                    .Where(x => request.Search == null || x.Name.Contains(request.Search!))
                    .Select(x => new CategoryResult
                    {
                        Id = x.Id,
                        Name = x.Name,
                        ImageUrl = x.ImageUrl
                    });

                // Pagination
                if (request.PageNumber.HasValue && request.PageSize.HasValue)
                {
                    query = query.Skip((request.PageNumber.Value - 1) * request.PageSize.Value).Take(request.PageSize.Value);
                }

                // Return the result
                return Task.FromResult(CommandResult<Result>.Success(new Result
                {
                    Results = query.ToList()
                }));
            }
        }
    }
}
