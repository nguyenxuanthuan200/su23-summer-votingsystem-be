using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Capstone_VotingSystem.Core.CoreModel;

namespace Capstone_VotingSystem.Controllers
{
    [Route("api/v1.0/[controller]s")]
    [ApiController]
    public class BaseApiController : ControllerBase
    {
        protected IActionResult SendResponse<T>(T response) where T : ApiResponse
        {
            if (response.Success == false)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        protected IActionResult SendResponse<T>(APIResponse<T> response) where T : class
        {
            if (response.Success == false)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        //protected IActionResult SendResponse<T>(APIResponse<T> response) where T : class
        //{
        //    if (response.Success == false)
        //    {
        //        return BadRequest(response);
        //    }
        //    return Ok(response);
        //}

        protected IActionResult SendResponse(ApiResponseListError response)
        {
            if (response.Success == false)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

    }
}
