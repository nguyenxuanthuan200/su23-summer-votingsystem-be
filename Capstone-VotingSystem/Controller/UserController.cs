using Capstone_VotingSystem.Models.RequestModels.UserRequest;
using Capstone_VotingSystem.Services.UserService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Capstone_VotingSystem.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _user;

        public UserController(IUserService userService)
        {
            this._user = userService;
        }
        //[Authorize(Roles = "Admin,User")]
        [HttpPut]
        [SwaggerOperation(summary: "Update persional profile by userId")]
        public async Task<IActionResult> UpdateUser(string? id, [FromForm] UpdateUserRequest request)
        {
            try
            {
                var result = await _user.UpdateUser(id, request);
                if (result.Success == false)
                {
                    return BadRequest(result.Message);
                }
                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from the database.");
            }
        }
    }
}
