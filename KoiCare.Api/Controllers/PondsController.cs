using KoiCare.Application.Features.Pond;
using KoiCare.Infrastructure.Middleware;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace KoiCare.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PondsController(IMediator mediator) : BaseController
    {
        [Auth]
        [HttpGet]
        public async Task<ActionResult<GetListPonds.Result>> GetListPonds()
        {
            var result = await mediator.Send(new GetListPonds.Query());
            return CommandResult(result);
        }
    }
}
