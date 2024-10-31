using KoiCare.Application.Commons;
using KoiCare.Application.Features.Order;
using KoiCare.Application.Features.Product;
using KoiCare.Infrastructure.Middleware;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace KoiCare.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : BaseController
    {
        private readonly IMediator _mediator;

        public OrderController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Auth("Admin")]
        [HttpGet("get-all")]
        public async Task<ActionResult<GetAllOrder.Result>> GetAllOrders([FromQuery] GetAllOrder.Query query)
        {
            var result = await _mediator.Send(query);
            return CommandResult(result);
        }

        [Auth]
        [HttpGet("{orderId}")]
        public async Task<ActionResult<GetOrderDetails.Result>> GetOrderDetails(int orderId)
        {
            var query = new GetOrderDetails.Query { OrderId = orderId };
            var result = await _mediator.Send(query);
            return CommandResult(result);
        }

        [Auth]
        [HttpPost("create")]
        public async Task<ActionResult<CommandResult<bool>>> CreateOrder([FromBody] CreateOrder.Command command)
        {
            var result = await _mediator.Send(command);
            return CommandResult(result);
        }

        private readonly ChangeStatusOrder.Handler _handler;
        [Auth("Admin")]
        [HttpPost("changestatus")]
        public async Task<IActionResult> ChangeStatus([FromBody] ChangeStatusOrder.Command command)
        {
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
            {
                return StatusCode((int)result.StatusCode, result.FailureReason);
            }

            return Ok(result.Payload);
        }
    }

}
