using KoiCare.Application.Abtractions.Database;
using KoiCare.Application.Abtractions.Localization;
using KoiCare.Application.Abtractions.LoggedUser;
using KoiCare.Application.Commons;
using KoiCare.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Net;

namespace KoiCare.Application.Features.Blogs
{
    public class UpdateBlog
    {
        public class Command : IRequest<CommandResult<Result>>
        {
            public required int Id { get; set; }
            public required string Title { get; set; }
            public required string Content { get; set; }
            public DateTime CreatedAt { get; internal set; }
        }

        public class Result
        {
            public string? Message { get; set; }
        }

        public class Handler(
            IAppLocalizer localizer,
            ILogger<UpdateBlog> logger,
            ILoggedUser loggedUser,
            IUnitOfWork unitOfWork,
            IRepository<BlogPost> blogRepos
            ) : BaseRequestHandler<Command, CommandResult<Result>>(localizer, logger, loggedUser, unitOfWork)
        {
            public override async Task<CommandResult<Result>> Handle(Command request, CancellationToken cancellationToken)
            {
                try
                {
                    var blog = await blogRepos.Queryable().FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
                    if (blog == null)
                    {
                        return CommandResult<Result>.Fail(_localizer["Blog not found"]);
                    }

                    blog.Title = request.Title;
                    blog.Content = request.Content;
                    blog.CreatedAt = request.CreatedAt;

                    blogRepos.Update(blog);
                    await _unitOfWork.SaveChangesAsync(cancellationToken);

                    return CommandResult<Result>.Success(new Result
                    {
                        Message = _localizer["Blog updated successfully"]
                    });
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, ex.Message);
                    return CommandResult<Result>.Fail(HttpStatusCode.InternalServerError, ex.Message);
                }
            }
        }
    }
}
