using Microsoft.AspNetCore.Mvc;
using RidePassAPI.Responses;

namespace RidePassAPI.Controllers
{
    [Route("errors/{code}")]
    [ApiExplorerSettings(IgnoreApi = true)]
    [ApiController]
    public class ErrorsController : ControllerBase
    {
        public IActionResult Error(int code)
        {
            return new ObjectResult(new ErrorResponse(code, "An error occured in processing your request!"));
        }
    }
}
