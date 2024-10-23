using KoiCare.Application.Features.Order;
using KoiCare.Application.Features.Product;
using KoiCare.Infrastructure.Middleware;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace KoiCare.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController(IMediator mediator) : BaseController
    {
        [Auth]
        [HttpGet("get-all")]
        public async Task<ActionResult<GetAllOrder.Result>> GetAllOrders([FromQuery] GetAllOrder.Query query)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await mediator.Send(query);
            return CommandResult(result);
        }

        [Auth]
        [HttpGet("{orderId}")]
        public async Task<ActionResult<GetOrderDetails.Result>> GetOrderDetails(int orderId)
        {
            var query = new GetOrderDetails.Query { OrderId = orderId };
            var result = await mediator.Send(query);
            return CommandResult(result);
        }
    }
}
