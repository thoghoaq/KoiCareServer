using KoiCare.Application.Abtractions.Database;
using KoiCare.Application.Abtractions.Localization;
using KoiCare.Application.Abtractions.LoggedUser;
using KoiCare.Application.Commons;
using KoiCare.Application.Features.Product;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace KoiCare.Application.Features.Koifish
{
    public class CreateKoiFish
    {
        public class Command : IRequest<CommandResult<Result>>
        {
            public required string Name { get; set; }
            public required int KoiTypeId { get; set; }
            public required int PondId { get; set; }
            public string? ImageUrl { get; set; }
            public decimal Age { get; set; }
            public decimal? Weight { get; set; }
            public int? Gender { get; set; }
            public int? Origin { get; set; }
            public int? Shape { get; set; }
            public decimal? Breed { get; set; }
            public decimal? Length { get; set; }
            //public required int CategoryId { get; set; }
        }

        public class Result
        {
            public string? Message { get; set; }
        }

        public class Handler(
           IRepository<Domain.Entities.KoiIndividual> koiRepos,
           IAppLocalizer localizer,
           ILogger<CreateKoiFish> logger,
           ILoggedUser loggedUser,
           IUnitOfWork unitOfWork
        ) : BaseRequestHandler<Command, CommandResult<Result>>(localizer, logger, loggedUser, unitOfWork)
        {
            private readonly IRepository<Domain.Entities.KoiIndividual> _koiRepos = koiRepos;

            public override async Task<CommandResult<Result>> Handle(Command request, CancellationToken cancellationToken)
            {
                try
                {
                    var koifish = new Domain.Entities.KoiIndividual()
                    {
                        Name = request.Name,
                        KoiTypeId = request.KoiTypeId,
                        PondId = request.PondId,
                        ImageUrl = request.ImageUrl,
                        Age = request.Age,
                        Weight = request.Weight,
                        Gender = request.Gender,
                        Origin = request.Origin,
                        Shape = request.Shape,
                        Length = request.Length,
                        Breed = request.Breed,
                        //CategoryId = request.CategoryId
                    };
                    
                    await _koiRepos.AddAsync(koifish, cancellationToken);
                    await _unitOfWork.SaveChangesAsync(cancellationToken);

                    return CommandResult<Result>.Success(new Result
                    {
                        Message = _localizer["Koi fish added successfully"],
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
