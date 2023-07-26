using Capstone_VotingSystem.Controllers;
using Capstone_VotingSystem.Models.RequestModels.UserRequest;
using Capstone_VotingSystem.Services.UserService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Capstone_VotingSystem.Controller
{
    [Route("api/v1/users")]
    [ApiController]
    public class UserController : BaseApiController
    {
        private readonly IUserService userService;
        public UserController(IUserService userService)
        {
            this.userService = userService;
        }
        [Authorize(Roles = "Admin,User")]
        [HttpPut]
        [SwaggerOperation(summary: "Update persional profile by userId")]
        public async Task<IActionResult> UpdateUser(string? id, [FromForm] UpdateUserRequest request)
        {
            try
            {
                var result = await userService.UpdateUser(id, request);
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
        [Authorize(Roles = "Admin,User")]
        [HttpPut("{id}/group/{groupid}")]
        [SwaggerOperation(summary: "Update Group User")]
        public async Task<IActionResult> UpdateUserGroup(string id, Guid groupid)
        {
            try
            {
                var result = await userService.UpdateUserGroup(id, groupid);
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
