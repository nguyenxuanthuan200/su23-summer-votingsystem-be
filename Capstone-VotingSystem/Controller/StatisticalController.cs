using Capstone_VotingSystem.Controllers;
using Capstone_VotingSystem.Services.StatisticalService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Capstone_VotingSystem.Controller
{
    [Route("api/v1/statisticals")]
    [ApiController]
    public class StatisticalController : BaseApiController
    {
        private readonly IStatisticalService statisticalService;
        public StatisticalController(IStatisticalService statisticalService)
        {
            this.statisticalService = statisticalService;
        }
        //[Authorize(Roles = "User,Admin")]
        [HttpGet("campaign/{campaignid}")]
        [SwaggerOperation(summary: "Get result of campaign")]
        public async Task<IActionResult> GetResultOfCampaign(Guid campaignid)
        {
            try
            {
                var result = await statisticalService.GetResultCampaign(campaignid);
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
        [HttpGet]
        [SwaggerOperation(summary: "Statistical Total")]
        public async Task<IActionResult> StatisticalTotal()
        {
            try
            {
                var result = await statisticalService.StatisticalTotal();
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
        [HttpGet("statistical-voter/{campaignid}")]
        [SwaggerOperation(summary: "Statistical Voter Join Campaign")]
        public async Task<IActionResult> StatisticalVoterJoinCampaign(Guid campaignid)
        {
            try
            {
                var result = await statisticalService.StatisticalVoterJoinCampaign(campaignid);
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
        [HttpGet("statistical-candidate/{campaignid}")]
        [SwaggerOperation(summary: "Statistical Vote Of Candidate Group")]
        public async Task<IActionResult> StatisticalVoteOfCandidateGroup(Guid campaignid)
        {
            try
            {
                var result = await statisticalService.StatisticalVoteOfCandidateGroup(campaignid);
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
