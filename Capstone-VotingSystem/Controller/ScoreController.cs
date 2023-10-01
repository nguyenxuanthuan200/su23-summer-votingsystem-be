using Capstone_VotingSystem.Controllers;
using Capstone_VotingSystem.Models.RequestModels.ScoreRequest;
using Capstone_VotingSystem.Services.ScoreService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Capstone_VotingSystem.Controller
{
    [Route("api/v1/scores")]
    [ApiController]
    public class ScoreController : BaseApiController
    {
        private readonly IScoreService scoreService;
        public ScoreController(IScoreService scoreService)
        {
            this.scoreService = scoreService;
        }
       // [Authorize(Roles = "User,Admin")]
        [HttpGet]
        [SwaggerOperation(summary: "Get Score")]
        public async Task<IActionResult> GetScore([FromQuery] GetScoreByCampaginRequest request)
        {
            try
            {
                var result = await scoreService.GetScore(request);
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
