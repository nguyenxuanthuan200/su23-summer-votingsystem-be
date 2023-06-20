using CoreApiResponse;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Capstone_VotingSystem.Models.RequestModels.CampaignRequest;
using Capstone_VotingSystem.Services.CampaignService;
using Swashbuckle.AspNetCore.Annotations;
using Capstone_VotingSystem.Controllers;
using Microsoft.AspNetCore.Authorization;

namespace Capstone_VotingSystem.Controller
{
    [Route("api/v1/campaigns")]
    [ApiController]
    public class CampaignController : BaseApiController
    {
        private readonly ICampaignService campaignService;
        public CampaignController(ICampaignService campaignService)
        {
            this.campaignService = campaignService;
        }
        //[Authorize(Roles = "User,Admin")]
        [HttpGet]
        [SwaggerOperation(summary: "Get all campaign")]
        public async Task<IActionResult> GetCampaign()
        {
            try
            {
                var result = await campaignService.GetCampaign();
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
        [Authorize(Roles = "User,Admin")]
        [HttpGet("{id}")]
        [SwaggerOperation(summary: "Get Campaign by Id")]
        public async Task<IActionResult> GetCampaignById(Guid id)
        {
            try
            {
                var result = await campaignService.GetCampaignById(id);
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
        [Authorize(Roles = "User,Admin")]
        [HttpGet("user/{Id}")]
        [SwaggerOperation(summary: "Get Campaign by User Id")]
        public async Task<IActionResult> GetCampaignByUserId(string Id)
        {
            try
            {
                var result = await campaignService.GetCampaignByUserId(Id);
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
        [SwaggerOperation(summary: "Create new Campaign")]
        public async Task<IActionResult> CreateCampaign(CreateCampaignRequest request)
        {
            try
            {
                var result = await campaignService.CreateCampaign(request);
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
        [SwaggerOperation(summary: "Update Campaign")]
        public async Task<IActionResult> UpdateCampaign(Guid id, UpdateCampaignRequest request)
        {
            try
            {
                var result = await campaignService.UpdateCampaign(id,request);
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
        [HttpPut]
        [SwaggerOperation(summary: "Update Visibility Campaign")]
        public async Task<IActionResult> UpdateVisibilityCampaign(Guid id,string visibility,string userId)
        {
            try
            {
                var result = await campaignService.UpdateVisibilityCampaign(id, visibility, userId);
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
        [Authorize(Roles = "User,Admin")]
        [HttpDelete("{campaignId}")]
        [SwaggerOperation(summary: "Delete Campaign")]
        public async Task<IActionResult> DeleteCampaign(Guid campaignId, DeleteCampaignRequest request)
        {
            try
            {
                var result = await campaignService.DeleteCampaign(campaignId,request);
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
