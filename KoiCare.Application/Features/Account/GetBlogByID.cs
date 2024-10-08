using KoiCare.Application.Abtractions.Database;
using KoiCare.Application.Abtractions.Localization;
using KoiCare.Application.Commons;
using KoiCare.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using KoiCare.Application.Abtractions.LoggedUser;

namespace KoiCare.Application.Features.Blogs
{
    public class GetBlogById
    {
        public class Query : IRequest<CommandResult<Result>>
        {
            public int Id { get; set; }
        }

        public class Result
        {
            public int Id { get; set; }
            public string Title { get; set; } = string.Empty;
            public string Content { get; set; } = string.Empty;
            public string Author { get; set; } = string.Empty;
            public DateTime CreatedAt { get; set; }
        }

        public class Handler(
            IAppLocalizer localizer,
            ILogger<GetBlogById> logger,
            ILoggedUser loggedUser,
            IUnitOfWork unitOfWork,
            IRepository<BlogPost> blogRepos
            ) : BaseRequestHandler<Query, CommandResult<Result>>(localizer, logger, loggedUser, unitOfWork)
        {
            public override async Task<CommandResult<Result>> Handle(Query request, CancellationToken cancellationToken)
            {
                var blog = await blogRepos.Queryable()
                    .Include(x => x.Author)
                    .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

                if (blog == null)
                {
                    return CommandResult<Result>.Fail("Blog không tồn tại");
                }

                var result = new Result
                {
                    Id = blog.Id,
                    Title = blog.Title,
                    Content = blog.Content,
                    Author = blog.Author.Username,
                    CreatedAt = blog.CreatedAt
                };

                return CommandResult<Result>.Success(result);
            }
        }
    }
}
