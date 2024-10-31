using KoiCare.Application.Abtractions.Database;
using KoiCare.Application.Abtractions.Localization;
using KoiCare.Application.Abtractions.LoggedUser;
using KoiCare.Application.Commons;
using KoiCare.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Net;

namespace KoiCare.Application.Features.Blog
{
    public class CreateBlog
    {
        public class Command : IRequest<CommandResult<Result>>
        {
            public required string Title { get; set; }
            public required string Content { get; set; }
        }

        public class Result
        {
            public string? Message { get; set; }
        }

        public class Handler(
            IRepository<BlogPost> blogPostRepository,
            IAppLocalizer localizer,
            ILogger<CreateBlog> logger,
            ILoggedUser loggedUser,
            IUnitOfWork unitOfWork
        ) : BaseRequestHandler<Command, CommandResult<Result>>(localizer, logger, loggedUser, unitOfWork)
        {
            private readonly IRepository<BlogPost> _blogPostRepository = blogPostRepository;

            public override async Task<CommandResult<Result>> Handle(Command request, CancellationToken cancellationToken)
            {
                try
                {
                    // Tạo blog mới
                    var blogPost = new BlogPost
                    {
                        Title = request.Title,
                        Content = request.Content,
                        AuthorId = _loggedUser.UserId, // Lấy ID người dùng hiện tại
                        CreatedAt = DateTime.UtcNow,
                    };

                    // Thêm blog vào cơ sở dữ liệu
                    await _blogPostRepository.AddAsync(blogPost, cancellationToken);
                    await _unitOfWork.SaveChangesAsync(cancellationToken);

                    return CommandResult<Result>.Success(new Result
                    {
                        Message = _localizer["Blog post created successfully"],
                    });
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "CreateBlogError");
                    return CommandResult<Result>.Fail(HttpStatusCode.InternalServerError, ex.Message);
                }
            }
        }
    }
}
