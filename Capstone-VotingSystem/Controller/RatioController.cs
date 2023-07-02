using Capstone_VotingSystem.Entities;
using Capstone_VotingSystem.Models.RequestModels.RatioRequest;
using Capstone_VotingSystem.Services.RatioService;
using CoreApiResponse;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace Capstone_VotingSystem.Controller
{
    [Route("api/v1/ratios")]
    [ApiController]
    public class RatioController : BaseController
    {
        private readonly IRatioService ratioService;

        public RatioController(IRatioService ratioService)
        {
            this.ratioService = ratioService;
        }
        [Authorize(Roles = "User,Admin")]
        [HttpGet("campaign/{id}")]
        [SwaggerOperation(summary: "Get all Ratio By Campaign (role user and admin)")]
        public async Task<IActionResult> GetRatioByCampaign(Guid id)
        {
            try
            {
                var result = await ratioService.GetRatioByCampaign(id);
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
        [SwaggerOperation(summary: "Create new Ratio")]
        public async Task<IActionResult> CreateRatio(CreateRatioRequest request)
        {
            try
            {
                var result = await ratioService.CreateCampaignRatio(request);
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
        [SwaggerOperation(summary: "Update Ratio")]
        public async Task<IActionResult> UpdateRatio(Guid id, UpdateRatioRequest request)
        {
            try
            {
                var result = await ratioService.UpdateRatio(id, request);
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
