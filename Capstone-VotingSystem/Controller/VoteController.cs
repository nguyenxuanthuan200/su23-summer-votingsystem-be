using Microsoft.AspNetCore.Mvc;
using Capstone_VotingSystem.Models.RequestModels.VoteRequest;
using Capstone_VotingSystem.Services.VoteService;
using Microsoft.AspNetCore.Authorization;
using Capstone_VotingSystem.Controllers;
using Swashbuckle.AspNetCore.Annotations;
using Capstone_VotingSystem.Models.RequestModels.ElementRequest;
using Capstone_VotingSystem.Services.QuestionService;
using Capstone_VotingSystem.Models.RequestModels.VoteDetailRequest;
using Capstone_VotingSystem.Models.RequestModels.VotingDetailRequest;

namespace Capstone_VotingSystem.Controller
{
    [Route("api/v1/votes")]
    [ApiController]
    public class VoteController : BaseApiController
    {
        private readonly IVoteService voteService;
        public VoteController(IVoteService voteService)
        {
            this.voteService = voteService;
        }
      // [Authorize(Roles = "User")]
        [HttpPost]
        [SwaggerOperation(summary: "Create new vote have form ")]
        public async Task<IActionResult> CreateVote(CreateVoteRequest request)
        {
            try
            {
                var result = await voteService.CreateVote(request);
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
        [HttpGet("statistical")]
        [SwaggerOperation(summary: "Statistical Vote by campaign ")]
        public async Task<IActionResult> StatisticalVoteByCampaign([FromQuery] StatisticalVoteRequest request)
        {
            try
            {
                var result = await voteService.StatisticalVoteByCampaign(request);
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
        [HttpPost("like")]
        [SwaggerOperation(summary: "Create new vote Like ")]
        public async Task<IActionResult> CreateVoteLike(CreateVoteLikeRequest request)
        {
            try
            {
                var result = await voteService.CreateVoteLike(request);
                if (result.Success == false)
                {
                    return BadRequest(result);
                }
                return Ok(result);
            }
            catch (Exception e)
            {
                //return StatusCode(StatusCodes.Status500InternalServerError,
                //    "Error retrieving data from the database.");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    e.Message);
            }
        }
    }
}
