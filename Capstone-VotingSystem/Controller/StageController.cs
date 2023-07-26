using Capstone_VotingSystem.Controllers;
using Capstone_VotingSystem.Models.RequestModels.StageRequest;
using Capstone_VotingSystem.Services.StageService;
using CoreApiResponse;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace Capstone_VotingSystem.Controller
{
    [Route("api/v1/stages")]
    [ApiController]
    public class StageController : BaseApiController
    {
        private readonly IStageService campaignStageService;
        public StageController(IStageService campaignStageService)
        {
            this.campaignStageService = campaignStageService;
        }
        //[Authorize(Roles = "User,Admin")]
        [SwaggerOperation(summary: "Get Stage By Campaign")]
        [HttpGet("campaign/{id}")]
        public async Task<IActionResult> GetCampaignStage(Guid id)
        {
            try
            {
                var result = await campaignStageService.GetCampaignStageByCampaign(id);
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

        //
        [SwaggerOperation(summary: "checkdate stage")]
        [HttpGet("{starttime},{endtime},{newstarttime},{newendtime}")]
        public async Task<IActionResult> checkdatestage(DateTime starttime, DateTime endtime, DateTime newstarttime, DateTime newendtime)
        {
            try
            {
                DateTime startTime = starttime;
                DateTime endTime = endtime;
                DateTime newStartTime =newstarttime;
                DateTime newEndTime = newendtime;
                TimeZoneInfo vnTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");

                // Chuyển đổi giá trị của hai biến datetime mới sang múi giờ của Việt Nam
               // DateTime newStartVn = TimeZoneInfo.ConvertTimeFromUtc(newStartTime.ToUniversalTime(), vnTimeZone);
               // DateTime newEndVn = TimeZoneInfo.ConvertTimeFromUtc(newEndTime.ToUniversalTime(), vnTimeZone);

                var check = DateTime.Compare(newStartTime, startTime);
                var check1 = DateTime.Compare(newEndTime, endTime) <= 0;
                if (DateTime.Compare(newEndTime, newStartTime) <= 0 )
                {
                    return BadRequest();
                    //Console.WriteLine("Start time is after current time, and end time is after start time.");
                }
                // Kiểm tra giá trị của hai biến DateTime mới
                if (DateTime.Compare(newStartTime, startTime) >= 0 && DateTime.Compare(newEndTime, endTime) <= 0)
                {
                    //Console.WriteLine("New start time and new end time are within the range of start time and end time.");
                }
                else
                {
                    //response.ToFailedResponse("Thời gian bắt đầu của giai đoạn và thời gian kết thúc của giai đoạn không nằm trong phạm vi của thời gian bắt đầu và thời gian kết thúc của chiến dịch.", StatusCodes.Status400BadRequest);
                    //return response;
                    return BadRequest();
                }
                return Ok();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from the database.");
            }
        }




        //
        [Authorize(Roles = "User")]
        [HttpPost]
        [SwaggerOperation(summary: "Create new Stage")]
        public async Task<IActionResult> CreateCampaignStage(CreateStageRequest request)
        {
            try
            {
                var result = await campaignStageService.CreateCampaignStage(request);
                if (result.Success == false)
                {
                    return BadRequest(result);
                }
                return Ok(result);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                   e.Message);
            }
        }
        [Authorize(Roles = "User")]
        [HttpPut("{id}")]
        [SwaggerOperation(summary: "Update Stage")]
        public async Task<IActionResult> UpdateCampaignStage(Guid id,UpdateStageRequest request)
        {
            try
            {
                var result = await campaignStageService.UpdateCampaignStage(id,request);

                if (result.Success == false)
                {
                    return BadRequest(result);
                }
                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error updating Store!");
            }

        }
    }
}
