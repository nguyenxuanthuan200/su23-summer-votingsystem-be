﻿using Capstone_VotingSystem.Controllers;
using Capstone_VotingSystem.Services.FeedbackService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Capstone_VotingSystem.Controller
{
    [Route("api/v1/feedbacks")]
    [ApiController]
    public class FeedbackController : BaseApiController
    {
        private readonly IFeedbackService _feedback;

        public FeedbackController(IFeedbackService feedbackService)
        {
            this._feedback = feedbackService;
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        [SwaggerOperation(summary: "Get All FeedBack")]
        public async Task<IActionResult> GetAllFeedback()
        {
            try
            {
                var result = await _feedback.GetAllFeedback();
                if (result.Success == false)
                {
                    return BadRequest(result.Message);
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
        [HttpGet("{id}")]
        [SwaggerOperation(summary: "Get Feedback By Id")]
        public async Task<IActionResult> GetFeedBack(Guid? id)
        {
            try
            {
                var result = await _feedback.GetByFeedBackId(id);
                if (result.Success == false)
                {
                    return BadRequest(result.Message);
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