using Capstone_VotingSystem.Controllers;
using Capstone_VotingSystem.Models.RequestModels.TypeActionRequest;
using Capstone_VotingSystem.Services.ActionTypeService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Capstone_VotingSystem.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActionTypeController : BaseApiController
    {
        private readonly IActionTypeService _actionType;

        public ActionTypeController(IActionTypeService actionTypeService)
        {
            this._actionType = actionTypeService;
        }
        //[Authorize(Roles = "User")]
        [HttpGet]
        [SwaggerOperation(summary: "Get All ActionType")]
        public async Task<IActionResult> GetActionHistoryByUser()
        {
            try
            {
                var result = await _actionType.GetAll();
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
        [HttpPost]
        [SwaggerOperation(summary: "Create New ActionType")]
        public async Task<IActionResult> CreateActionType(ActionTypeRequest request)
        {
            try
            {
                var result = await _actionType.CreateTypeAction(request);
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
