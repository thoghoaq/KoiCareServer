using KoiCare.Application.Features.KoiGroup;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace KoiCare.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class KoiGroupController(IMediator mediator) : BaseController
    {
        [HttpGet]
        public async Task<IActionResult> GetKoiGroups([FromQuery] GetKoiGroups.Query query)
        {
            var result = await mediator.Send(query);
            return CommandResult(result);
        }
    }
}
