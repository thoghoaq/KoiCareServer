using KoiCare.Application.Features.Product;
using KoiCare.Infrastructure.Middleware;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace KoiCare.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController(IMediator mediator) : BaseController
    {
        [HttpGet("get-all")]
        public async Task<ActionResult<GetAllProduct.Result>> GetAllProducts([FromQuery] GetAllProduct.Query query)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await mediator.Send(query);
            return CommandResult(result);
        }

        [Auth("Admin")]
        [HttpPost("create")]
        public async Task<ActionResult<CreateProduct.Result>> CreateProduct([FromBody] CreateProduct.Command command)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await mediator.Send(command);
            return CommandResult(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GetProductByID.Result>> GetProductByID(int id)
        {
            var query = new GetProductByID.Query { Id = id };
            var result = await mediator.Send(query);
            return CommandResult(result);
        }

        [Auth("Admin")]
        [HttpPut("{id}")]
        public async Task<ActionResult<UpdateProduct.Result>> UpdateProduct(int id, [FromBody] UpdateProduct.Command command)
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
        public async Task<ActionResult<DeleteProduct.Result>> DeleteProduct(int id)
        {
            var command = new DeleteProduct.Command { Id = id };
            var result = await mediator.Send(command);
            return CommandResult(result);
        }
    }
}
