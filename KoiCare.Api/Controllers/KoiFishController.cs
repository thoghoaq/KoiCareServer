using KoiCare.Application.Features.Koifish;
using KoiCare.Application.Features.Product;
using KoiCare.Infrastructure.Middleware;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace KoiCare.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KoiFishController(IMediator mediator) : BaseController
    {

        [HttpGet("get-all-koi-types")]
        public async Task<ActionResult<GetAllKoiType.Result>> GetAllKoiTypes([FromQuery] GetAllKoiType.Query query)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await mediator.Send(query);
            return CommandResult(result);
        }

        [Auth]
        [HttpGet("get-all-fish/{PondId}")]
        public async Task<ActionResult<GetAllFishInPond.Result>> GetAllFishInPond(int PondId)
        {
            var query = new GetAllFishInPond.Query { PondId = PondId };
            var result = await mediator.Send(query);
            return CommandResult(result);
        }

        [Auth("User")]
        [HttpPost("create")]
        public async Task<ActionResult<CreateKoiFish.Result>> CreateKoiFish([FromBody] CreateKoiFish.Command command)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await mediator.Send(command);
            return CommandResult(result);
        }

        [Auth("User")]
        [HttpPut("{id}")]
        public async Task<ActionResult<UpdateKoiFish.Result>> UpdateKoiFish(int id, [FromBody] UpdateKoiFish.Command command)
        {
            if (id != command.Id)
            {
                return BadRequest("ID mismatch");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await mediator.Send(command);
            return CommandResult(result);
        }

        [Auth("User")]
        [HttpGet("{id}")]
        public async Task<ActionResult<GetKoiFishById.Result>> GetKoiFishById(int id)
        {
            var query = new GetKoiFishById.Query { Id = id };
            var result = await mediator.Send(query);
            return CommandResult(result);
        }
    }
}
