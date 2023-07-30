using Capstone_VotingSystem.Controllers;
using Capstone_VotingSystem.Models.RequestModels.GroupRequest;
using Capstone_VotingSystem.Services.GroupService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Capstone_VotingSystem.Controller
{
    [Route("api/v1/groups")]
    [ApiController]
    public class GroupController : BaseApiController
    {
        private readonly IGroupService groupService;
        public GroupController(IGroupService groupService)
        {
            this.groupService = groupService;
        }
        //[Authorize(Roles = "User,Admin")]
        [HttpGet]
        [SwaggerOperation(summary: "Get all Group (role user and admin)")]
        public async Task<IActionResult> GetGroup()
        {
            try
            {
                var result = await groupService.GetListGroup();
                if (result.Success == false)
                {
                    return BadRequest(result);
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
        [HttpPost]
        [SwaggerOperation(summary: "Create new Group (role admin)")]
        public async Task<IActionResult> CreateGroup(CreateGroupRequest request)
        {
            try
            {
                var result = await groupService.CreateGroup(request);
                if (result.Success == false)
                {
                    return BadRequest(result);
                }
                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from the database.");
            }
        }
        //[Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        [SwaggerOperation(summary: "Update Group (role admin)")]
        public async Task<IActionResult> UpdateGroup(Guid id, UpdateGroupRequest request)
        {
            try
            {
                var result = await groupService.UpdateGroup(id, request);
                if (result.Success == false)
                {
                    return BadRequest(result);
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
