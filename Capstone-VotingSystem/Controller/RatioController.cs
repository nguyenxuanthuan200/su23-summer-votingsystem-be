using Capstone_VotingSystem.Entities;
using Capstone_VotingSystem.Repositories.RateCategoryRepo;
using CoreApiResponse;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Capstone_VotingSystem.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class RatioController : BaseController
    {
        private readonly IRatioRepositories ratio;

        public RatioController(IRatioRepositories ratioRepositories) 
        {
            this.ratio = ratioRepositories;
        }
        [HttpGet("getAll")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await ratio.GetAllRatio();
                if (result == null)
                    return CustomResult("Not Found", HttpStatusCode.NotFound);
                return CustomResult("Success", result, HttpStatusCode.OK);
            }
            catch (Exception)
            {
                return CustomResult("Fail", HttpStatusCode.InternalServerError);
            }
        }
        [HttpGet("GetById")]
        public async Task<IActionResult> GetCandidateProfile(Guid id)
        {
            try
            {
                var result = await ratio.GetRatioById(id);
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
