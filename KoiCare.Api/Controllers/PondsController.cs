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
        /// <summary>
        /// Get list ponds of the logged user, if admin get all ponds
        /// </summary>
        /// <returns></returns>
        [Auth]
        [HttpGet]
        public async Task<ActionResult<GetListPonds.Result>> GetListPonds()
        {
            var result = await mediator.Send(new GetListPonds.Query());
            return CommandResult(result);
        }

        /// <summary>
        /// Create or update pond, remove id to create new pond
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [Auth]
        [HttpPost("save")]
        public async Task<ActionResult<CreateUpdatePond.Result>> CreateUpdatePond([FromBody] CreateUpdatePond.Command command)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await mediator.Send(command);
            return CommandResult(result);
        }

        [Auth]
        [HttpGet("{id}")]
        public async Task<ActionResult<GetPondDetail.Result>> GetPond([FromRoute] int id)
        {
            var query = new GetPondDetail.Query { Id = id };
            var result = await mediator.Send(query);
            return CommandResult(result);
        }
    }
}
