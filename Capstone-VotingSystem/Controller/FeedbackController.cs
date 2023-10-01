using Capstone_VotingSystem.Controllers;
using Capstone_VotingSystem.Models.RequestModels.FeedbackRequest;
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
        // [Authorize(Roles = "Admin")]
        //[HttpGet]
        //[SwaggerOperation(summary: "Get All FeedBack")]
        //public async Task<IActionResult> GetAllFeedback()
        //{
        //    try
        //    {
        //        var result = await _feedback.GetAllFeedback();
        //        if (result.Success == false)
        //        {
        //            return BadRequest(result.Message);
        //        }
        //        return Ok(result);

        //    }
        //    catch (Exception)
        //    {
        //        return StatusCode(StatusCodes.Status500InternalServerError,
        //             "Error retrieving data from the database.");
        //    }
        //}
        //[Authorize(Roles = "User")]
        [HttpGet("user/{userid}")]
        [SwaggerOperation(summary: "Get Feedback By UserId")]
        public async Task<IActionResult> GetFeedbackByUserId(string userid)
        {
            try
            {
                var result = await _feedback.GetFeedbackByUserId(userid);
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
        [HttpGet("user/{userid}/campaign/{campaignid}")]
        [SwaggerOperation(summary: "Check Feedback for User")]
        public async Task<IActionResult> CheckFeedback(string userid,Guid campaignid)
        {
            try
            {
                var result = await _feedback.CheckFeedback(userid,campaignid);
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
        [HttpGet("campaign/{campaignid}")]
        [SwaggerOperation(summary: "Get Feedback By CampaignId")]
        public async Task<IActionResult> GetFeedBackByCampaignId(Guid campaignid)
        {
            try
            {
                var result = await _feedback.GetFeedBackByCampaignId(campaignid);
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
        [HttpPost]
        [SwaggerOperation(summary: "Create Feedback Campaign")]
        public async Task<IActionResult> CreateFeedback(FeedbackRequest request)
        {
            try
            {
                var result = await _feedback.CreateFeedback(request);
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
        [HttpDelete]
        [SwaggerOperation(summary: "Delete Feedback")]
        public async Task<IActionResult> DeleteFeedback(Guid? feedbackid, DeleteFeedbackRequest request)
        {
            try
            {
                var result = await _feedback.DeleteFeedback(feedbackid, request);
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
