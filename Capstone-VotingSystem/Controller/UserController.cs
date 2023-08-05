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
        // [Authorize(Roles = "Admin,User")]
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
        // [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        [SwaggerOperation(summary: "Update User Permission")]
        public async Task<IActionResult> UpdateUserPermission(string id, [FromForm] UserPermissionRequest request)
        {
            try
            {
                var result = await userService.UpdateUserPermission(id, request);
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
        // [Authorize(Roles = "Admin")]
        [HttpGet]
        [SwaggerOperation(summary: "Get All User To Manage")]
        public async Task<IActionResult> GetAllUser()
        {
            try
            {
                var result = await userService.GetAllUser();
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
        // [Authorize(Roles = "User")]
        [HttpPut("{userid}/group/{groupid}/campaign/{campaignid}")]
        [SwaggerOperation(summary: "Update or add new Group User for Campaign")]
        public async Task<IActionResult> UpdateUserGroup(string userid, Guid groupid, Guid campaignid)
        {
            try
            {
                var result = await userService.UpdateUserGroup(userid, groupid, campaignid);
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
        [HttpPut("userid")]
        [SwaggerOperation(summary: "Update Image for User")]
        public async Task<IActionResult> AddUserImage(IFormFile formFile, string? userid)
        {
            try
            {
                var folderName = "user";
                var result = await userService.AddImageUserAsync(formFile, folderName, userid);
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
