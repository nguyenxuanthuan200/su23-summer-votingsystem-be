using Capstone_VotingSystem.Entities;
using Capstone_VotingSystem.Models.RequestModels.CandidateProfile;
using Capstone_VotingSystem.Repositories.CandidateProfileRepo;
using CoreApiResponse;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Capstone_VotingSystem.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class CandidateProfileController : BaseController
    {
        private readonly ICandidateProfileService candidateProfile;

        public CandidateProfileController(ICandidateProfileService candidateProfileRepositories) 
        {
            this.candidateProfile = candidateProfileRepositories;
        }
        [Authorize(Roles = "User")]
        [HttpGet("GetAllCandidate")]
        
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await candidateProfile.GetAll();
                if (result == null)
                    return CustomResult("Not Found", HttpStatusCode.NotFound);
                return CustomResult("Success", result, HttpStatusCode.OK);
            }
            catch (Exception)
            {
                return CustomResult("Fail", HttpStatusCode.InternalServerError);
            }
        }
        
        [HttpGet("GetCandidateProfileCampaign")]
        public async Task<IActionResult> GetCandidateProfile(Guid campaign)
        {
            try
            {
                var result = await candidateProfile.GetCandidateByCampaign(campaign);
                if (result == null)
                    return CustomResult("Not Found", HttpStatusCode.NotFound);
                return CustomResult("Success", result, HttpStatusCode.OK);
            }
            catch (Exception)
            {
                return CustomResult("Fail", HttpStatusCode.InternalServerError);
            }
        }
        [HttpPut("UpdateCandidateProfile")]
        public async Task<IActionResult> UpdateCandidateProfile(Guid id, UpdateCandidateProfile profile)
        {
            try
            {
                var result = await candidateProfile.UpdateCandidate(id, profile);
                if (result == null)
                    return CustomResult("Not Found", HttpStatusCode.NotFound);
                return CustomResult("Success", result, HttpStatusCode.OK);
            }
            catch (Exception) 
            {
                return CustomResult("Fail", HttpStatusCode.InternalServerError);
            }
        }
    }
}
