using Capstone_VotingSystem.Controllers;
using Capstone_VotingSystem.Core.CoreModel;
using Capstone_VotingSystem.Models.RequestModels.CandidateRequest;
using Capstone_VotingSystem.Models.ResponseModels.CandidateResponse;
using Capstone_VotingSystem.Services.CandidateService;
using CoreApiResponse;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Swashbuckle.AspNetCore.Annotations;


namespace Capstone_VotingSystem.Controller
{
    [Route("api/v1.0/candidates")]
    [ApiController]
    public class CandidateController : BaseApiController
    {
        private readonly ICandidateService candidateService;
        public CandidateController(ICandidateService candidateService)
        {
            this.candidateService = candidateService;
        }

        [HttpGet("campaign/{id}")]
        [SwaggerOperation(summary: "Get list candidate by campaign id")]
        public async Task<IActionResult> getListCandidateCampaign(Guid campaignid)
        {
            try
            {
                var result = await candidateService.GetListCandidateCampaign(campaignid);
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

        [HttpPost("account")]
        [SwaggerOperation(summary: "Add account Candidate to Campagin with some info of Candidate")]
        public async Task<IActionResult> CreateAccountCandidateCampaign(CreateAccountCandidateRequest request)
        {
            try
            {
                var response = await candidateService.CreateAccountCandidateCampaign(request);
                if (response.Success == false)
                {
                    return BadRequest(response);
                }
                return Ok(response);
            }
            catch (Exception)
            {
                return BadRequest();


            }
        }
        [HttpPost]
        [SwaggerOperation(summary: "Add Candidate to Campagin with some info of Candidate")]
        public async Task<IActionResult> CreateCandidateCampaign(CreateCandidateCampaignRequest request)
        {
            try
            {
                APIResponse<GetCandidateCampaignResponse> response = await candidateService.CreateCandidateCampaign(request);
                if (response.Success == false)
                {
                    return BadRequest(response);
                }
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);

            }
        }
    }
}
