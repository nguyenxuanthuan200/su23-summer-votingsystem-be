using Capstone_VotingSystem.Models.RequestModels.ActionHistoryRequest;
using Capstone_VotingSystem.Services.ActionHistoryService;
using CoreApiResponse;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace Capstone_VotingSystem.Controller
{
    [Route("api/v1/actionhistories")]
    [ApiController]
    public class ActionHistoryController : BaseController
    {
        private readonly IActionHistoryService actionHistory;

        public ActionHistoryController(IActionHistoryService actionHistoryRepositories)
        {
            this.actionHistory = actionHistoryRepositories;
        }
        [Authorize(Roles = "User")]
        [HttpGet("{id}")]
        [SwaggerOperation(summary: "Get Action History by UserId Role User")]
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
        [Authorize(Roles = "User")]
        [HttpPost]
        [SwaggerOperation(summary: "Create ActionHistory")]
        public async Task<IActionResult> CreateActionHistory(ActionHistoryRequest request)
        {
            try
            {
                var result = await actionHistory.CreateActionHistory(request);
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
        [SwaggerOperation(summary: "Update ActionHistory")]
        public async Task<IActionResult> UpdateActionHistory(UpdateActionHistoryRequest request, Guid? id)
        {
            try
            {
                var result = await actionHistory.UpdateActionHistory(request, id);
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
