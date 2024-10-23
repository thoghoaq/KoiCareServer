using KoiCare.Application.Abtractions.Database;
using KoiCare.Application.Abtractions.Localization;
using KoiCare.Application.Abtractions.LoggedUser;
using KoiCare.Application.Commons;
using KoiCare.Application.Features.Category;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static KoiCare.Application.Features.Product.GetAllProduct;

namespace KoiCare.Application.Features.Koifish
{
    public class GetAllKoiType
    {
        public record KoiTypeResult
        {
            public int Id { get; set; }
            public required string Name { get; set; }
            public string? Description { get; set; }
        }

        public class Result
        {
            public List<KoiTypeResult> KoiTypes { get; set; } = new List<KoiTypeResult>();
        }

        public class Query : IRequest<CommandResult<Result>>
        {
            public int? PageNumber { get; set; }
            public int? PageSize { get; set; }
            public string? Search { get; set; }
        }

        public class Handler(
            IAppLocalizer localizer,
            ILogger<GetAllKoiType> logger,
            ILoggedUser loggedUser,
            IUnitOfWork unitOfWork,
            IRepository<Domain.Entities.KoiType> koiTypeRepos
            ) : BaseRequestHandler<Query, CommandResult<Result>>(localizer, logger, loggedUser, unitOfWork)
        {
            public override Task<CommandResult<Result>> Handle(Query request, CancellationToken cancellationToken)
            {
                var query = koiTypeRepos.Queryable()
                    .Include(x => x.PerfectWaterVolumes) // Giả sử có liên kết với bảng PerfectWaterVolumes
                    .Where(x => request.Search == null || x.Name.Contains(request.Search!))
                    .Select(x => new KoiTypeResult
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Description = x.Description
                    });
                // Phân trang
                if (request.PageNumber.HasValue && request.PageSize.HasValue)
                {
                    query = query.Skip((request.PageNumber.Value - 1) * request.PageSize.Value).Take(request.PageSize.Value);
                }

                // Trả về kết quả
                return Task.FromResult(CommandResult<Result>.Success(new Result
                {
                    KoiTypes = query.ToList()
                }));
            }
        }
    }
}
