using KoiCare.Application.Abtractions.Database;
using KoiCare.Application.Abtractions.Localization;
using KoiCare.Application.Abtractions.LoggedUser;
using KoiCare.Application.Commons;
using KoiCare.Application.Features.Category;
using KoiCare.Application.Features.Order;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static KoiCare.Application.Features.Order.GetOrderDetails;
using static KoiCare.Application.Features.Product.GetAllProduct;

namespace KoiCare.Application.Features.Koifish
{
    public class GetAllFishInPond
    {
        public record KoiResult
        {
            public int Id { get; set; }
            public int KoiTypeId { get; set; }
            public string? KoiType { get; set; }
            public int PondId { get; set; }
            public string? PondName { get; set; }
            public required string Name { get; set; }
            public string? ImageUrl { get; set; }
            public decimal? Age { get; set; }
            public decimal? Length { get; set; }
            public decimal? Weight { get; set; }
            public int? Gender { get; set; }
            public int? Origin { get; set; }
            public int? Shape { get; set; }
            public decimal? Breed { get; set; }
            //public int CategoryId { get; set; }
            //public string? CategoryName { get; set; }
        }

        public class Query : IRequest<CommandResult<Result>>
        {
            public int PondId { get; set; }
        }

        public class Result
        {
            public List<KoiResult> KoiResults { get; set; } = [];
        }

        public class Handler(
            IAppLocalizer localizer,
            ILogger<GetAllFishInPond> logger,
            ILoggedUser loggedUser,
            IUnitOfWork unitOfWork,
            IRepository<Domain.Entities.KoiIndividual> koiRepos
            ) : BaseRequestHandler<Query, CommandResult<Result>>(localizer, logger, loggedUser, unitOfWork)
        {
            public override Task<CommandResult<Result>> Handle(Query request, CancellationToken cancellationToken)
            {
                var query = koiRepos.Queryable()
                    .Include(x => x.Pond)
                    .Include(x => x.KoiType)
                    .Where(x => x.PondId.Equals(request.PondId))
                    .Select(x => new KoiResult
                    {
                        Id = x.Id,
                        Name = x.Name,
                        KoiTypeId = x.KoiTypeId,
                        KoiType = x.KoiType.Name,
                        PondId = x.PondId,
                        PondName = x.Pond.Name,
                        ImageUrl = x.ImageUrl,
                        Gender = x.Gender,
                        Weight = x.Weight,
                        Age = x.Age,
                        Length = x.Length,
                        Origin = x.Origin,
                        Shape = x.Shape,
                        Breed = x.Breed,
                        //CategoryId = x.CategoryId,
                        //CategoryName = x.Category.Name,
                    });

                // Trả về kết quả
                return Task.FromResult(CommandResult<Result>.Success(new Result
                {
                    KoiResults = query.ToList()
                }));
            }
        }
    }
}
