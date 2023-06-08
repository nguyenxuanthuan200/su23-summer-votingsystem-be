using Capstone_VotingSystem.Controllers;
using Capstone_VotingSystem.Models.RequestModels.FormRequest;
using Capstone_VotingSystem.Services.FormService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Capstone_VotingSystem.Controller
{
    [Route("api/v1.0/forms")]
    [ApiController]
    public class FormController : BaseApiController
    {
        private readonly IFormService formService;
        public FormController(IFormService formService)
        {
            this.formService = formService;
        }
        [HttpGet]
        [SwaggerOperation(summary: "Get all Form")]
        public async Task<IActionResult> GetCampaign()
        {
            try
            {
                var result = await formService.GetAllForm();
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
        [SwaggerOperation(summary: "Create new Form")]
        public async Task<IActionResult> CreateForm(CreateFormRequest request)
        {
            try
            {
                var result = await formService.CreateForm(request);
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
        [HttpPut("{id}")]
        [SwaggerOperation(summary: "Update Form")]
        public async Task<IActionResult> UpdateForm(Guid id, UpdateFormByUser request)
        {
            try
            {
                var result = await formService.UpdateForm(id, request);
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

