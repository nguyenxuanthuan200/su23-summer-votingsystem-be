using Capstone_VotingSystem.Models.RequestModels.CandidateRequest;
using Capstone_VotingSystem.Repositories.CandidateRepo;
using CoreApiResponse;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Capstone_VotingSystem.Controller
{
    [Route("api/candidate")]
    [ApiController]
    public class CandidateController : BaseController
    {
        private readonly ICandidateRepositories candidateRepositories;
        public CandidateController(ICandidateRepositories candidateRepositories)
        {
            this.candidateRepositories = candidateRepositories;
        }
        [HttpGet("campaignId")]
        public async Task<IActionResult> GetListCandidateCampaign(Guid campaignId)
        {
            try
            {
                var result = await candidateRepositories.GetListCandidateCampaign(campaignId);
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
        public async Task<IActionResult> CreateAccountCandidateCampaign(CreateAccountCandidateRequest request)
        {
            try
            {
                if (request == null)
                {
                    return CustomResult("Cu Phap Sai", HttpStatusCode.BadRequest);
                }
                var create = await candidateRepositories.CreateAccountCandidateCampaign(request);
                if (create == null)
                {
                    return CustomResult("account candidate thuoc campaign da ton tai", HttpStatusCode.Accepted);
                }
                //var result = _mapper.Map<CreateAccountResponse>(create);
                return CustomResult("Success", create, HttpStatusCode.Created);
            }
            catch (Exception)
            {
                return CustomResult("Fail", HttpStatusCode.InternalServerError);


            }
        }
        [HttpPost]
        public async Task<IActionResult> CreateCandidateCampaign(CreateCandidateCampaignRequest request)
        {
            try
            {
                if (request == null)
                {
                    return CustomResult("Cu Phap Sai", HttpStatusCode.BadRequest);
                }
                var create = await candidateRepositories.CreateCandidateCampaign(request);
                if (create == null)
                {
                    return CustomResult("candidate da ton tai", HttpStatusCode.Accepted);
                }
                //var result = _mapper.Map<CreateAccountResponse>(create);
                return CustomResult("Success", create, HttpStatusCode.Created);
            }
            catch (Exception)
            {
                return CustomResult("Fail", HttpStatusCode.InternalServerError);


            }
        }
    }
}
