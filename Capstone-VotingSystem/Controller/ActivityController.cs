using Capstone_VotingSystem.Controllers;
using Capstone_VotingSystem.Models.RequestModels.ActivityRequest;
using Capstone_VotingSystem.Services.ActivityService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Capstone_VotingSystem.Controller
{
    [Route("api/v1/activitys")]
    [ApiController]
    public class ActivityController : BaseApiController
    {
        private readonly IActivityService activityService;
        public ActivityController(IActivityService activityService)
        {
            this.activityService = activityService;
        }
        [Authorize(Roles = "User,Admin")]
        [HttpGet("candidate/{id}")]
        [SwaggerOperation(summary: "Get activity by candidateId")]
        public async Task<IActionResult> GetActivityByCandidateId(Guid id)
        {
            try
            {
                var result = await activityService.GetActivityByCandidate(id);
                if (result.Success == false)
                {
                    return BadRequest(result);
                }
                return Ok(result);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
        [Authorize(Roles = "User")]
        [HttpPost]
        [SwaggerOperation(summary: "Create new Activity")]
        public async Task<IActionResult> CreateActivity(CreateActivityRequest request)
        {
            try
            {
                var result = await activityService.CreateActivity(request);
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
        [Authorize(Roles = "User")]
        [HttpPut("{id}")]
        [SwaggerOperation(summary: "Update Activity")]
        public async Task<IActionResult> UpdateActivity(Guid id, UpdateActivityRequest request)
        {
            try
            {
                var result = await activityService.UpdateActivity(id, request);
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
        [Authorize(Roles = "User")]
        [HttpDelete("{id}")]
        [SwaggerOperation(summary: "Delete Activity")]
        public async Task<IActionResult> DeleteActivity(Guid id, DeleteActivityRequest request)
        {
            try
            {
                var result = await activityService.DeleteActivity(id, request);
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
