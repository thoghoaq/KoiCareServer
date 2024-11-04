using KoiCare.Application.Abtractions.Database;
using KoiCare.Application.Abtractions.Localization;
using KoiCare.Application.Abtractions.LoggedUser;
using KoiCare.Application.Commons;
using KoiCare.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace KoiCare.Application.Features.Blog
{
    public class GetAllBlog
    {
        public class Query : IRequest<CommandResult<Result>>
        {
            public int? PageNumber { get; set; }
            public int? PageSize { get; set; }
            public string? Search { get; set; }
        }

        public class Result
        {
            public List<BlogResult> Blogs { get; set; } = new List<BlogResult>();
        }

        public record BlogResult
        {
            public int Id { get; set; }
            public required string Title { get; set; }
            public required string Content { get; set; }
            public string? Image { get; set; }
            public required string AuthorName { get; set; }
            public DateTime CreatedAt { get; internal set; }
        }

        public class Handler(
            IAppLocalizer localizer,
            ILogger<GetAllBlog> logger,
            ILoggedUser loggedUser,
            IUnitOfWork unitOfWork,
            IRepository<BlogPost> blogRepos
            ) : BaseRequestHandler<Query, CommandResult<Result>>(localizer, logger, loggedUser, unitOfWork)
        {
            public override Task<CommandResult<Result>> Handle(Query request, CancellationToken cancellationToken)
            {
                // Query for blogs
                var query = blogRepos.Queryable()
                    .Include(x => x.Author) // Assuming there's an Author relationship
                    .Where(x => request.Search == null || x.Title.Contains(request.Search!) || x.Content.Contains(request.Search!))
                    .Select(x => new BlogResult
                    {
                        Id = x.Id,
                        Title = x.Title,
                        Content = x.Content,
                        Image = x.Image,
                        CreatedAt = x.CreatedAt,
                        AuthorName = x.Author.Username, // Assuming Author has Username
                    });

                // Pagination
                if (request.PageNumber.HasValue && request.PageSize.HasValue)
                {
                    query = query.Skip((request.PageNumber.Value - 1) * request.PageSize.Value).Take(request.PageSize.Value);
                }

                // Return the result
                return Task.FromResult(CommandResult<Result>.Success(new Result
                {
                    Blogs = query.ToList()
                }));
            }
        }
    }
}
