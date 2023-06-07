using Capstone_VotingSystem.Services.ActionHistoryService;
using CoreApiResponse;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Capstone_VotingSystem.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActionHistoryController : BaseController
    {
        private readonly IActionHistoryService actionHistory;

        public ActionHistoryController(IActionHistoryService actionHistoryRepositories)
        {
            this.actionHistory = actionHistoryRepositories;
        }
        //[Authorize(Roles = "Admin")]
        [HttpGet("GetAllActionHistory")]
        public async Task<IActionResult> GetActionHistory()
        {
            try
            {
                var result = await actionHistory.GetAllActionHistory();
                if (result == null)
                    return CustomResult("Not Found", HttpStatusCode.NotFound);
                return CustomResult("Success", result, HttpStatusCode.OK);
            }
            catch (Exception)
            {
                return CustomResult("Fail", HttpStatusCode.InternalServerError);
            }
        }
        [HttpGet("GetActionHistoryUser")]
        public async Task<IActionResult> GetActionHistoryByUser(string? username)
        {
            try
            {
                var result = await actionHistory.GetActionHistoryByUser(username);
                if (result == null)
                    return CustomResult("Not Found", HttpStatusCode.NotFound);
                return CustomResult("Success", result, HttpStatusCode.OK);
            }
            catch (Exception)
            {
                return CustomResult("Fail", HttpStatusCode.InternalServerError);
            }
        }
    }
}
