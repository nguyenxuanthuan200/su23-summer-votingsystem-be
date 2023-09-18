using Capstone_VotingSystem.Controllers;
using Capstone_VotingSystem.Services.ScripDemoService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Capstone_VotingSystem.Controller
{
    [Route("api/v1/scrip")]
    [ApiController]
    public class ScripDemoController : BaseApiController
    {
        private readonly IScripDemoService scripDemoService;
        public ScripDemoController(IScripDemoService scripDemoService)
        {
            this.scripDemoService = scripDemoService;
        }
       // [Authorize(Roles = "Admin")]
        [HttpGet("campaign/{campaignId}/stage/{stageId}")]
        [SwaggerOperation(summary: "Scrip demo vote")]
        public async Task<IActionResult> ScripVote(Guid campaignId,Guid stageId)
        {
            try
            {
                var result = await scripDemoService.ScripVote(campaignId,stageId);
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
        [HttpGet("campaign/{campaignId}")]
        [SwaggerOperation(summary: "Scrip end process of campaign")]
        public async Task<IActionResult> ScripEndProcess(Guid campaignId)
        {
            try
            {
                var result = await scripDemoService.ScripEndCampaign(campaignId);
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
