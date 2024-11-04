using KoiCare.Application.Features.Image;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace KoiCare.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController(IMediator mediator) : BaseController
    {
        [HttpPost("upload")]
        public async Task<ActionResult<UploadImage.Result>> UploadImage([FromForm] UploadImage.Command command)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await mediator.Send(command);
            return CommandResult(result);
        }
    }
}
