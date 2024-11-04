using KoiCare.Application.Features.Category;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace KoiCare.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController(IMediator mediator) : BaseController
    {
        [HttpGet("get-all")]
        public async Task<ActionResult<GetAllCategory.Result>> GetAllCategorys([FromQuery] GetAllCategory.Query query)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await mediator.Send(query);
            return CommandResult(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GetCategoryByID.Result>> GetCategoryById(int id)
        {
            var query = new GetCategoryByID.Query { Id = id };
            var result = await mediator.Send(query);
            return CommandResult(result);
        }
    }
}
