using KoiCare.Application.Commons;
using KoiCare.Application.Features.Order;
using KoiCare.Infrastructure.Middleware;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace KoiCare.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController(IMediator mediator) : BaseController
    {
        [Auth("Admin")]
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
        public async Task<ActionResult<GetOrderDetails.Result>> GetOrderDetails(int orderId, [FromServices] IMediator mediator)
        {
            var query = new GetOrderDetails.Query { OrderId = orderId };

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await mediator.Send(query);
            return CommandResult(result);
        }

        [Auth]
        [HttpPost("create")]
        public async Task<ActionResult<CommandResult<bool>>> CreateOrder([FromBody] CreateOrder.Command command)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await mediator.Send(command);
            return CommandResult(result);
            {
                // Trả về Payload chứa dữ liệu trả về khi thành công
                return Ok(result.Payload);  // Đây là cách trả về kết quả thành công từ Payload
            }
            else
            {
                // Trả về lỗi với FailureReason khi không thành công
                return StatusCode((int)(result.StatusCode ?? HttpStatusCode.BadRequest), result.FailureReason);  // Trả về lý do thất bại
            }
        }


        [Auth("Admin")]
        [HttpPost("changestatus")]
        public async Task<ActionResult<ChangeStatusOrder.Result>> ChangeStatus([FromBody] ChangeStatusOrder.Command command)
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
