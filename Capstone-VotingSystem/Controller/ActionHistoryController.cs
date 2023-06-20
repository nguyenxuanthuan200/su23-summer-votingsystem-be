using Capstone_VotingSystem.Services.ActionHistoryService;
using CoreApiResponse;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace Capstone_VotingSystem.Controller
{
    [Route("api/v1/actionhistory")]
    [ApiController]
    public class ActionHistoryController : BaseController
    {
        private readonly IActionHistoryService actionHistory;

        public ActionHistoryController(IActionHistoryService actionHistoryRepositories)
        {
            this.actionHistory = actionHistoryRepositories;
        }
        [Authorize(Roles = "User")]
        [HttpGet("{userid}")]
        [SwaggerOperation(summary: "Get Action History by UserId")]
        public async Task<IActionResult> GetActionHistoryByUser(string? userId)
        {
            try
            {
                var result = await actionHistory.GetActionHistoryByUser(userId);
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
