using Capstone_VotingSystem.Controllers;
using Capstone_VotingSystem.Services.ActionHistoryService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Capstone_VotingSystem.Controller
{
    [Route("api/v1/action-histories")]
    [ApiController]
    public class ActionHistoryController : BaseApiController
    {
        private readonly IActionHistoryService actionHistory;

        public ActionHistoryController(IActionHistoryService actionHistoryRepositories)
        {
            this.actionHistory = actionHistoryRepositories;
        }
        [Authorize(Roles = "User")]
        [HttpGet("user/{id}")]
        [SwaggerOperation(summary: "Get Action History by UserId")]
        public async Task<IActionResult> GetActionHistoryByUser(string? id)
        {
            try
            {
                var result = await actionHistory.GetActionHistoryByUser(id);
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
