using Capstone_VotingSystem.Controllers;
using Capstone_VotingSystem.Models.RequestModels.CampaignStageRequest;
using Capstone_VotingSystem.Services.CampaignStageService;
using CoreApiResponse;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace Capstone_VotingSystem.Controller
{
    [Route("api/v1/campaignstage")]
    [ApiController]
    public class CampaignStageController : BaseApiController
    {
        private readonly ICampaignStageService campaignStageService;
        public CampaignStageController(ICampaignStageService campaignStageService)
        {
            this.campaignStageService = campaignStageService;
        }
        [Authorize(Roles = "User,Admin")]
        [SwaggerOperation(summary: "Get CampaignStage By Campaign")]
        [HttpGet]
        public async Task<IActionResult> GetCampaignStage(Guid campaignId)
        {
            try
            {
                var result = await campaignStageService.GetCampaignStageByCampaign(campaignId);
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
        [HttpPost]
        [SwaggerOperation(summary: "Create new CampaignStage")]
        public async Task<IActionResult> CreateCampaignStage(CreateCampaignStageRequest request)
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
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                   e.Message);
            }
        }
        [Authorize(Roles = "User")]
        [HttpPut("{id}")]
        [SwaggerOperation(summary: "Update CampaignStage")]
        public async Task<IActionResult> UpdateCampaignStage(Guid id,UpdateCampaignStageRequest request)
        {
            try
            {
                var result = await campaignStageService.UpdateCampaignStage(id,request);

                if (result.Success == false)
                {
                    return BadRequest(result);
                }
                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error updating Store!");
            }

        }
    }
}
