using Capstone_VotingSystem.Entities;
using Capstone_VotingSystem.Services.RateCategoryService;
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
        private readonly IRatioCategoryService ratioCategoryService;

        public RatioController(IRatioCategoryService ratioCategoryService)
        {
            this.ratioCategoryService = ratioCategoryService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await ratioCategoryService.GetAllRatio();
                if (result == null)
                    return CustomResult("Not Found", HttpStatusCode.NotFound);
                return CustomResult("Success", result, HttpStatusCode.OK);
            }
            catch (Exception)
            {
                return CustomResult("Fail", HttpStatusCode.InternalServerError);
            }
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCandidateProfile(Guid id)
        {
            try
            {
                var result = await ratioCategoryService.GetRatioById(id);
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
