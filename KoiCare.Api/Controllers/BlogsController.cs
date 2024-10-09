using KoiCare.Application.Features.Blog;
using KoiCare.Infrastructure.Middleware;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace KoiCare.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogsController(IMediator mediator) : BaseController
    {
        [Auth]
        [HttpGet("get-all")]
        public async Task<ActionResult<GetAllBlog.Result>> GetAllBlogs([FromQuery] GetAllBlog.Query query)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await mediator.Send(query);
            return CommandResult(result);
        }

        [Auth]
        [HttpGet("{id}")]
        public async Task<ActionResult<GetBlogById.Result>> GetBlogById(int id)
        {
            var query = new GetBlogById.Query { Id = id };
            var result = await mediator.Send(query);
            return CommandResult(result);
        }

        [Auth("Admin")]
        [HttpPost("create")]
        public async Task<ActionResult<CreateBlog.Result>> CreateBlog([FromBody] CreateBlog.Command command)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await mediator.Send(command);
            return CommandResult(result);
        }

        [Auth("Admin")]
        [HttpPut("{id}")]
        public async Task<ActionResult<UpdateBlog.Result>> UpdateBlog(int id, [FromBody] UpdateBlog.Command command)
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
        [HttpDelete("{id}")]
        public async Task<ActionResult<DeleteBlog.Result>> DeleteBlog(int id)
        {
            var command = new DeleteBlog.Command { Id = id };
            var result = await mediator.Send(command);
            return CommandResult(result);
        }
    }
}
