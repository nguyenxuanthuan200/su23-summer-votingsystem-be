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
        [HttpGet("campaign/{id}")]
        [SwaggerOperation(summary: "Get list group by campaign")]
        public async Task<IActionResult> GetGroupByCampagin(Guid id)
        {
            try
            {
                var result = await groupService.GetListGroupByCampaign(id);
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
        //[Authorize(Roles = "User,Admin")]
        [HttpGet("user/{id}/campaign/{camid}")]
        [SwaggerOperation(summary: "Check Group of Voter for campaign")]
        public async Task<IActionResult> CheckGroupUser(string id,Guid camid)
        {
            try
            {
                var result = await groupService.CheckGroupUser(id,camid);
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
        //[Authorize(Roles = "User,Admin")]
        [HttpGet("statistical/campaign/{campaignid}")]
        [SwaggerOperation(summary: "Statistical group by campaign")]
        public async Task<IActionResult> StatisticalGroupByCampaign(Guid campaignid)
        {
            try
            {
                var result = await groupService.StatisticalGroupByCampaign(campaignid);
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
        [SwaggerOperation(summary: "Create new Group for campaign")]
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
        [SwaggerOperation(summary: "Update Group for campaign")]
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
