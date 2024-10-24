using KoiCare.Application.Features.Feeding;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace KoiCare.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FeedingController(IMediator mediator) : BaseController
    {
        [HttpGet("{KoiIndividualId}")]
        public async Task<ActionResult<GetFeedingSchedule.Result>> GetFeedingSchedule(int KoiIndividualId)
        {
            var query = new GetFeedingSchedule.Query { KoiIndividualId = KoiIndividualId };
            var result = await mediator.Send(query);
            return CommandResult(result);
        }

        [HttpPost("create")]
        public async Task<ActionResult<CreateFeedingSchedule.Result>> CreateFeedingSchedule([FromBody] CreateFeedingSchedule.Command command)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await mediator.Send(command);
            return CommandResult(result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<UpdateFeedingSchedule.Result>> UpdateFeedingSchedule(int id, [FromBody] UpdateFeedingSchedule.Command command)
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

        [HttpPost("feed-calculation")]
        public async Task<ActionResult<CalculateFeedingAmount.Result>> CalculateFeedingAmount([FromBody] CalculateFeedingAmount.Command command)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await mediator.Send(command);
            return CommandResult(result);
        }

        [HttpGet("feed-calculation")]
        public async Task<ActionResult<GetFeedingCalculation.QueryResult>> CalculateFeedingAmount([FromQuery] GetFeedingCalculation.Query query)
        {
            var result = await mediator.Send(query);
            return CommandResult(result);
        }
    }
}
