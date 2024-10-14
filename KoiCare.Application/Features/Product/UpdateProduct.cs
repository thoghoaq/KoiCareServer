using KoiCare.Application.Abtractions.Database;
using KoiCare.Application.Abtractions.Localization;
using KoiCare.Application.Abtractions.LoggedUser;
using KoiCare.Application.Commons;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Net;

namespace KoiCare.Application.Features.Product
{
    public class UpdateProduct
    {
        public class Result
        {
            public string? Message { get; set; }
        }

        public class Command : IRequest<CommandResult<Result>>
        {
            public required int Id { get; set; }
            public required string Name { get; set; }
            public required string Description { get; set; }
            public required decimal Price { get; set; }
            public required string ImageUrl { get; set; }
            public required int CategoryId { get; set; }
        }

        public class Handler(
            IAppLocalizer localizer,
            ILogger<UpdateProduct> logger,
            ILoggedUser loggedUser,
            IUnitOfWork unitOfWork,
            IRepository<Domain.Entities.Product> productRepos
            ) : BaseRequestHandler<Command, CommandResult<Result>>(localizer, logger, loggedUser, unitOfWork)
        {
            public override async Task<CommandResult<Result>> Handle(Command request, CancellationToken cancellationToken)
            {
                try
                {
                    //Tìm Product trong cơ sở dữ liệu
                    var product = await productRepos.Queryable().FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
                    if (product == null)
                    {
                        return CommandResult<Result>.Fail(_localizer["Product not found"]);
                    }
                    //Update Product
                    product.Name = request.Name;
                    product.Description = request.Description;
                    product.Price = request.Price;
                    product.ImageUrl = request.ImageUrl;
                    product.CategoryId = request.CategoryId;

                    productRepos.Update(product);
                    await _unitOfWork.SaveChangesAsync(cancellationToken);

                    return CommandResult<Result>.Success(new Result
                    {
                        Message = _localizer["Product updated successfully"]
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
