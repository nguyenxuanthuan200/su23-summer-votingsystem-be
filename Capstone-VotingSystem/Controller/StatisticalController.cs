using Capstone_VotingSystem.Controllers;
using Capstone_VotingSystem.Services.StatisticalService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Capstone_VotingSystem.Controller
{
    [Route("api/v1/statistical")]
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
        [SwaggerOperation(summary: "Get Statistica lTotal")]
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
    }
}
