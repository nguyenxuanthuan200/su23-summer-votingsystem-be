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
    [Route("api/v1/vote")]
    [ApiController]
    public class VoteController : BaseApiController
    {
        private readonly IVoteService voteService;
        public VoteController(IVoteService voteService)
        {
            this.voteService = voteService;
        }
        [Authorize(Roles = "User")]
        [HttpPost]
        [SwaggerOperation(summary: "Create new vote ")]
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

    }
}
