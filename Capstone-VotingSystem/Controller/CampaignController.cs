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
        // [Authorize(Roles = "User,Admin")]
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
        // [Authorize(Roles = "User,Admin")]
        [HttpGet("update-process")]
        [SwaggerOperation(summary: "update process campaign and stage ")]
        public async Task<IActionResult> UpdateProcess()
        {
            try
            {
                var result = await campaignService.UpdateProcess();
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
        [HttpGet("user/{id}")]
        [SwaggerOperation(summary: "Get List Campaign by User Id")]
        public async Task<IActionResult> GetCampaignByUserId(string id)
        {
            try
            {
                var result = await campaignService.GetCampaignByUserId(id);
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
        [HttpGet("list-approve")]
        [SwaggerOperation(summary: "Get Campaign Need Approve")]
        public async Task<IActionResult> GetCampaignNeedApprove()
        {
            try
            {
                var result = await campaignService.GetCampaignNeedApprove();
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
        [HttpPost]
        [SwaggerOperation(summary: "Create new Campaign")]
        public async Task<IActionResult> CreateCampaign([FromForm] CreateCampaignRequest request)
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
        // [Authorize(Roles = "User")]
        [HttpPut("{id}")]
        [SwaggerOperation(summary: "Update Campaign")]
        public async Task<IActionResult> UpdateCampaign(Guid id, [FromForm] UpdateCampaignRequest request)
        {
            try
            {
                var result = await campaignService.UpdateCampaign(id, request);
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
        [HttpPut("approve/{campaignId}")]
        [SwaggerOperation(summary: "Approve Campaign")]
        public async Task<IActionResult> ApproveCampaign(Guid campaignId)
        {
            try
            {
                var result = await campaignService.ApproveCampaign(campaignId);
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
        [HttpDelete("{id}")]
        [SwaggerOperation(summary: "Delete Campaign")]
        public async Task<IActionResult> DeleteCampaign(Guid id, DeleteCampaignRequest request)
        {
            try
            {
                var result = await campaignService.DeleteCampaign(id, request);
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
        [HttpPut("campaignid")]
        [SwaggerOperation(summary: "Update Image Campaign")]
        public async Task<IActionResult> AddCampaignImage(IFormFile formFile, Guid? campaignid)
        {
            try
            {
                var folderName = "campaign";
                var result = await campaignService.AddImageCampaignAsync(formFile, folderName, campaignid);
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
