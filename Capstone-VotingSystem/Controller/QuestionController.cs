﻿using Capstone_VotingSystem.Controllers;
using Capstone_VotingSystem.Models.RequestModels.ElementRequest;
using Capstone_VotingSystem.Models.RequestModels.QuestionRequest;
using Capstone_VotingSystem.Services.QuestionService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Capstone_VotingSystem.Controller
{
    [Route("api/v1/questions")]
    [ApiController]
    public class QuestionController : BaseApiController
    {
        private readonly IQuestionService questionService;
        public QuestionController(IQuestionService questionService)
        {
            this.questionService = questionService;
        }
       // [Authorize(Roles = "User")]
        [HttpGet("form/{id}")]
        [SwaggerOperation(summary: "Get list question by form id")]
        public async Task<IActionResult> GetListQuestionForm(Guid id)
        {
            try
            {
                var result = await questionService.GetListQuestionForm(id);
                if (result.Success == false)
                {
                    return BadRequest(result);
                }
                return Ok(result);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
        // [Authorize(Roles = "User,Admin")]
        [HttpGet("{id}")]
        [SwaggerOperation(summary: "Get question by id")]
        public async Task<IActionResult> GetQuestionById(Guid id)
        {
            try
            {
                var result = await questionService.GetQuestionById(id);
                if (result.Success == false)
                {
                    return BadRequest(result);
                }
                return Ok(result);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
        // [Authorize(Roles = "User")]
        [HttpGet("form/{id}/total-question")]
        [SwaggerOperation(summary: "Get number question in form by form id")]
        public async Task<IActionResult> GetNumberQuestionInForm(Guid id)
        {
            try
            {
                var result = await questionService.GetNumberQuestionInForm(id);
                if (result.Success == false)
                {
                    return BadRequest(result);
                }
                return Ok(result);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
        // [Authorize(Roles = "User")]
        [HttpPost("elements")]
        [SwaggerOperation(summary: "Create new list question and element")]
        public async Task<IActionResult> CreateQuestion(CreateListQuestionRequest request)
        {
            try
            {
                var result = await questionService.CreateQuestion(request);
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
       //[Authorize(Roles = "User")]
        [HttpPost]
        [SwaggerOperation(summary: "Create new question rating type")]
        public async Task<IActionResult> CreateQuestionNoElement(CreateQuestionWithNoElementRequest request)
        {
            try
            {
                var result = await questionService.CreateQuestionNoElement(request);
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
        [HttpPost("{id}/element")]
        [SwaggerOperation(summary: "Create new element for question")]
        public async Task<IActionResult> CreateElementQuestion(Guid id,CreateElementRequest request)
        {
            try
            {
                var result = await questionService.CreateElementQuestion(id,request);
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
        [HttpPut("{id}")]
        [SwaggerOperation(summary: "Update Question and Element")]
        public async Task<IActionResult> UpdateQuestion(Guid id, UpdateQuestionRequest request)
        {
            try
            {
                var result = await questionService.UpdateQuestion(id, request);
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
        [HttpDelete("{id}")]
        [SwaggerOperation(summary: "Delete Question")]
        public async Task<IActionResult> DeleteQuestion(Guid id)
        {
            try
            {
                var result = await questionService.DeleteQuestion(id);
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
