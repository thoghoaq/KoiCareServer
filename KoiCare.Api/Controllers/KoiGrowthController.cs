using KoiCare.Application.Features.Blog;
using KoiCare.Application.Features.Koifish;
using KoiCare.Application.Features.KoiGrowth;
using KoiCare.Infrastructure.Middleware;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace KoiCare.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KoiGrowthController(IMediator mediator) : BaseController
    {
        [Auth]
        [HttpGet("get-all-fish-growth/{KoiIndividualId}")]
        public async Task<ActionResult<GetAllKoiGrowth.Result>> GetAllKoiGrowth(int KoiIndividualId)
        {
            var query = new GetAllKoiGrowth.Query { KoiIndividualId = KoiIndividualId };
            var result = await mediator.Send(query);
            return CommandResult(result);
        }

        [Auth("User")]
        [HttpPost("create")]
        public async Task<ActionResult<CreateKoiGrowth.Result>> CreateKoiGrowth([FromBody] CreateKoiGrowth.Command command)
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
        public async Task<ActionResult<UpdateKoiGrowth.Result>> UpdateKoiFish(int id, [FromBody] UpdateKoiGrowth.Command command)
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
        [HttpDelete("{id}")]
        public async Task<ActionResult<DeleteKoiGrowth.Result>> DeleteKoiGrowth(int id)
        {
            var command = new DeleteKoiGrowth.Command { Id = id };
            var result = await mediator.Send(command);
            return CommandResult(result);
        }
    }
}
