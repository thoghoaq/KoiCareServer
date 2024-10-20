using KoiCare.Application.Abtractions.Database;
using KoiCare.Application.Abtractions.Localization;
using KoiCare.Application.Abtractions.LoggedUser;
using KoiCare.Application.Commons;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Net;

namespace KoiCare.Application.Features.Product
{
    public class CreateProduct
    {
        public class Command : IRequest<CommandResult<Result>>
        {
            public required string Name { get; set; }
            public required string Description { get; set; }
            public required decimal Price { get; set; }
            public required string ImageUrl { get; set; }
            public required int CategoryId { get; set; }
        }
        public class Result
        {
            public string? Message { get; set; }
        }

        public class Handler(
           IRepository<Domain.Entities.Product> productRepos,
           IAppLocalizer localizer,
           ILogger<CreateProduct> logger,
           ILoggedUser loggedUser,
           IUnitOfWork unitOfWork
        ): BaseRequestHandler<Command, CommandResult<Result>>(localizer, logger, loggedUser, unitOfWork)
        {
            private readonly IRepository<Domain.Entities.Product> _productRepos = productRepos;

            public override async Task<CommandResult<Result>> Handle(Command request, CancellationToken cancellationToken)
            {
                try
                {
                    // Tạo Product mới
                    var product = new Domain.Entities.Product()
                    {
                        Name = request.Name,
                        Description = request.Description,
                        Price = request.Price,
                        ImageUrl = request.ImageUrl,
                        CategoryId = request.CategoryId
                    };
                    // Thêm Product vào cơ sở dữ liệu
                    await _productRepos.AddAsync(product, cancellationToken);
                    await _unitOfWork.SaveChangesAsync(cancellationToken);

                    return CommandResult<Result>.Success(new Result
                    {
                        Message = _localizer["Product created successfully"],
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
