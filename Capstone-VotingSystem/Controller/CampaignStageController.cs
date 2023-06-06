using Capstone_VotingSystem.Models.RequestModels.CampaignStageRequest;
using Capstone_VotingSystem.Services.CampaignStageService;
using CoreApiResponse;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Capstone_VotingSystem.Controller
{
    [Route("api/campaignstage")]
    [ApiController]
    public class CampaignStageController : BaseController
    {
        private readonly ICampaignStageService campaignStageRepositories;
        public CampaignStageController(ICampaignStageService campaignStageRepositories)
        {
            this.campaignStageRepositories = campaignStageRepositories;
        }
        [HttpGet]
        public async Task<IActionResult> GetCampaignStage(Guid campaignId)
        {
            try
            {
                var result = await campaignStageRepositories.GetCampaignStageByCampaign(campaignId);
                if (result == null)
                    return CustomResult("Not Found", HttpStatusCode.NotFound);
                return CustomResult("Success", result, HttpStatusCode.OK);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from the database.");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateCampaignStage(CreateCampaignStageRequest request)
        {
            try
            {
                if (request == null)
                {
                    return CustomResult("Cu Phap Sai", HttpStatusCode.BadRequest);
                }
                var create = await campaignStageRepositories.CreateCampaignStage(request);
                if (create == null)
                {
                    return CustomResult("CampaignStage da ton tai", HttpStatusCode.Accepted);
                }
                return CustomResult("Success", create, HttpStatusCode.Created);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                   e.Message);
            }
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCampaignStage(UpdateCampaignStageRequest request)
        {
            try
            {
                //if (request == null)
                //{
                //    return CustomResult("Cu Phap Sai", HttpStatusCode.BadRequest);
                //}
                var update = await campaignStageRepositories.UpdateCampaignStage(request);

                if (update == null)
                {
                    return CustomResult("Not Found", HttpStatusCode.NotFound);
                }

                return CustomResult("Success", update, HttpStatusCode.OK);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error updating Store!");
            }

        }
    }
}
