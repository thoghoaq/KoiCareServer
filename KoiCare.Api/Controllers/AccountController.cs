using KoiCare.Application.Features.Account;
using KoiCare.Infrastructure.Middleware;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace KoiCare.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController(IMediator mediator) : BaseController
    {
        [HttpPost("register")]
        public async Task<ActionResult<CreateUser.Result>> Register([FromBody] CreateUser.Command command)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await mediator.Send(command);
            return CommandResult(result);
        }

        [HttpPost("login")]
        public async Task<ActionResult<LoginUser.Result>> Login([FromBody] LoginUser.Command command)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await mediator.Send(command);
            return CommandResult(result);
        }

        [Auth]
        [HttpGet("user")]
        public async Task<ActionResult<GetUser.Result>> GetUser([FromQuery] GetUser.Query query)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await mediator.Send(query);
            return CommandResult(result);
        }

        [HttpPost("forgot-password")]
        public async Task<ActionResult<ForgotPassword.Result>> ForgotPassword([FromBody] ForgotPassword.Command command)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await mediator.Send(command);
            return CommandResult(result);
        }


        [Auth]
        [HttpPost("refresh-token")]
        public async Task<ActionResult<RefreshToken.Result>> RefreshToken([FromBody] RefreshToken.Command command)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await mediator.Send(command);
            return CommandResult(result);
        }

        [Auth]
        [HttpGet("logged-user")]
        public async Task<ActionResult<GetUser.Result>> LoggedUser()
        {
            var result = await mediator.Send(new GetUser.Query());
            return CommandResult(result);
        }

        [Auth("Admin")]
        [HttpGet("all-users")]
        public async Task<ActionResult<GetAllUser.Result>> GetAllUsers([FromQuery] GetAllUser.Query query)
        {
            var result = await mediator.Send(query);
            return CommandResult(result);
        }

        [Auth]
        [HttpPut("update/{id}")]
        public async Task<ActionResult<UpdateUser.Result>> UpdateUser(int id, [FromBody] UpdateUser.Command command)
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

        [Auth("Admin")]
        [HttpPut("disable/{id}")]
        public async Task<ActionResult<DisableUser.Result>> DisableUser(int id, [FromBody] DisableUser.Command command)
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
    }
}
