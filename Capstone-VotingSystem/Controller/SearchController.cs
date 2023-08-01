using Capstone_VotingSystem.Models.RequestModels.SearchRequest;
using Capstone_VotingSystem.Services.SearchService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Capstone_VotingSystem.Controller
{
    [Route("api/v1/search")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        private readonly ISearchService searchService;
        public SearchController(ISearchService searchService)
        {
            {
                this.searchService = searchService;
            }
        }

        [HttpGet("campaign")]
        public async Task<IActionResult> SearchFilterCampaign([FromQuery] SearchCampaignRequest payload)
        {
            try
            {

                var result = await searchService.SearchFilterCampaign(payload);

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
        [HttpGet("candidate")]
        public async Task<IActionResult> SearchFilterCandidate([FromQuery] SearchCandidateRequest payload)
        {
            try
            {

                var result = await searchService.SearchFilterCandidate(payload);

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
