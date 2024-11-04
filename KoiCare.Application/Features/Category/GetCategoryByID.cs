using KoiCare.Application.Abtractions.Database;
using KoiCare.Application.Abtractions.Localization;
using KoiCare.Application.Abtractions.LoggedUser;
using KoiCare.Application.Commons;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace KoiCare.Application.Features.Category
{
    public class GetCategoryByID
    {
        public class Result
        {
            public int Id { get; set; }
            public string Name { get; set; } = string.Empty;
            public string? ImageUrl { get; set; }
        }
        public class Query : IRequest<CommandResult<Result>>
        {
            public int Id { get; set; }
        }

        public class Handler(
           IAppLocalizer localizer,
           ILogger<GetCategoryByID> logger,
           ILoggedUser loggedUser,
           IUnitOfWork unitOfWork,
           IRepository<Domain.Entities.Category> cateRepos
           ) : BaseRequestHandler<Query, CommandResult<Result>>(localizer, logger, loggedUser, unitOfWork)
        {
            public override async Task<CommandResult<Result>> Handle(Query request, CancellationToken cancellationToken)
            {
                var category = await cateRepos.Queryable()
                    .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

                if (category == null)
                {
                    return CommandResult<Result>.Fail("Category does not exist");
                }

                var result = new Result
                {
                    Id = category.Id,
                    Name = category.Name,
                    ImageUrl = category.ImageUrl
                };

                return CommandResult<Result>.Success(result);
            }
        }
    }
}
