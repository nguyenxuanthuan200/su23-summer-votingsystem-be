using Capstone_VotingSystem.Controllers;
using Capstone_VotingSystem.Models.RequestModels.TypeRequest;
using Capstone_VotingSystem.Services.TypeService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Capstone_VotingSystem.Controller
{
    [Route("api/v1/types")]
    [ApiController]
    public class TypeController : BaseApiController
    {
        private readonly ITypeService typeService;
        public TypeController(ITypeService typeService)
        {
            this.typeService = typeService;
        }
        [Authorize(Roles = "User,Admin")]
        [HttpGet]
        [SwaggerOperation(summary: "Get all type (role user and admin)")]
        public async Task<IActionResult> GetType()
        {
            try
            {
                var result = await typeService.GetListType();
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
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [SwaggerOperation(summary: "Create new Type (role admin)")]
        public async Task<IActionResult> CreateType(CreateTypeRequest request)
        {
            try
            {
                var result = await typeService.CreateType(request);
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
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        [SwaggerOperation(summary: "Update Type (role admin)")]
        public async Task<IActionResult> UpdateCampaign(Guid id, UpdateTypeRequest request)
        {
            try
            {
                var result = await typeService.UpdateType(id, request);
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
