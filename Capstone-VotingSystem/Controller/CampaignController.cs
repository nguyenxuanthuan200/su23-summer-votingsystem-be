using CoreApiResponse;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Capstone_VotingSystem.Repositories.CampaignRepo;
using System.Net;

namespace Capstone_VotingSystem.Controller
{
    [Route("api/campaign")]
    [ApiController]
    public class CampaignController : BaseController
    {
        private readonly ICampaignRepositories campaignRepositories;
        public CampaignController(ICampaignRepositories campaignRepositories)
        {
            this.campaignRepositories = campaignRepositories;
        }
        [HttpGet]
        public async Task<IActionResult> GetCampaign()
        {
            try
            {
                var result = await campaignRepositories.GetCampaign();
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
    }
}
