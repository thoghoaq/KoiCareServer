using KoiCare.Application.Features.Account;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace KoiCare.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController(IMediator mediator) : BaseController
    {
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] CreateUser.Command command)
        {
            var result = await mediator.Send(command);
            return CommandResult(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUser.Command command)
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
