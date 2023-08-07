using Capstone_VotingSystem.Controllers;
using Capstone_VotingSystem.Models.RequestModels.StageRequest;
using Capstone_VotingSystem.Services.StageService;
using CoreApiResponse;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace Capstone_VotingSystem.Controller
{
    [Route("api/v1/stages")]
    [ApiController]
    public class StageController : BaseApiController
    {
        private readonly IStageService campaignStageService;
        public StageController(IStageService campaignStageService)
        {
            this.campaignStageService = campaignStageService;
        }
        //[Authorize(Roles = "User,Admin")]
        [SwaggerOperation(summary: "Get Stage By Campaign")]
        [HttpGet("campaign/{id}")]
        public async Task<IActionResult> GetCampaignStage(Guid id)
        {
            try
            {
                var result = await campaignStageService.GetCampaignStageByCampaign(id);
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
        [SwaggerOperation(summary: "Get Stage By Id")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetStageById(Guid id)
        {
            try
            {
                var result = await campaignStageService.GetStageById(id);
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

        //[Authorize(Roles = "User")]
        [HttpPost]
        [SwaggerOperation(summary: "Create new Stage")]
        public async Task<IActionResult> CreateCampaignStage(CreateStageRequest request)
        {
            try
            {
                var result = await campaignStageService.CreateCampaignStage(request);
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
        // [Authorize(Roles = "User")]
        [HttpPut("{id}")]
        [SwaggerOperation(summary: "Update Stage")]
        public async Task<IActionResult> UpdateCampaignStage(Guid id, UpdateStageRequest request)
        {
            try
            {
                var result = await campaignStageService.UpdateCampaignStage(id, request);

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
        //[Authorize(Roles = "User")]
        [SwaggerOperation(summary: "Delete Stage")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStage(Guid id)
        {
            try
            {
                var result = await campaignStageService.DeleteStage(id);
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
