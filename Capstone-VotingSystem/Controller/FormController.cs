using Capstone_VotingSystem.Controllers;
using Capstone_VotingSystem.Models.RequestModels.FormRequest;
using Capstone_VotingSystem.Services.FormService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Capstone_VotingSystem.Controller
{
    [Route("api/v1/forms")]
    [ApiController]
    public class FormController : BaseApiController
    {
        private readonly IFormService formService;
        public FormController(IFormService formService)
        {
            this.formService = formService;
        }
       // [Authorize(Roles = "User,Admin")]
        [HttpGet]
        [SwaggerOperation(summary: "Get all Form public and is approve")]
        public async Task<IActionResult> GetForm()
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
       // [Authorize(Roles = "User,Admin")]
        [HttpGet("{id}")]
        [SwaggerOperation(summary: "Get Form By Id")]
        public async Task<IActionResult> GetFormById(Guid id)
        {
            try
            {
                var result = await formService.GetFormById(id);
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
      //  [Authorize(Roles = "User,Admin")]
        [HttpGet("user/{id}")]
        [SwaggerOperation(summary: "Get Form By User Id")]
        public async Task<IActionResult> GetFormByUserId(string id)
        {
            try
            {
                var result = await formService.GetFormByUserId(id);
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
       // [Authorize(Roles = "User,Admin")]
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
       // [Authorize(Roles = "User,Admin")]
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
       // [Authorize(Roles = "User,Admin")]
        [HttpDelete("{id}")]
        [SwaggerOperation(summary: "Delete Form")]
        public async Task<IActionResult> DeleteForm(Guid id,DeleteFormRequest request)
        {
            try
            {
                var result = await formService.DeleteForm(id,request);
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
        //[Authorize(Roles = "Admin")]
        [HttpGet("list-approve")]
        [SwaggerOperation(summary: "Get Form Need Approve")]
        public async Task<IActionResult> GetFormNeedApprove()
        {
            try
            {
                var result = await formService.GetFormNeedApprove();
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
        // [Authorize(Roles = "Admin")]
        [HttpPut("approve/{formid}")]
        [SwaggerOperation(summary: "Approve Form")]
        public async Task<IActionResult> ApproveForm(Guid formid)
        {
            try
            {
                var result = await formService.ApproveForm(formid);
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

