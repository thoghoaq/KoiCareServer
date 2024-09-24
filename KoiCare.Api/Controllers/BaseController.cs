using KoiCare.Application.Commons;
using Microsoft.AspNetCore.Mvc;

namespace KoiCare.Api.Controllers
{
    public abstract class BaseController : ControllerBase
    {
        public ActionResult CommandResult<T>(CommandResult<T> result)
        {
            if (result.StatusCode != null)
            {
                return StatusCode((int)result.StatusCode, result.FailureDetails);
            }
            return result.IsSuccess ? Ok(result.Payload) : BadRequest(result.FailureDetails);
        }
    }
}
