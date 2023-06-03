using CoreApiResponse;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Capstone_VotingSystem.Repositories.VoteRepo;
using System.Net;
using Capstone_VotingSystem.Models.RequestModels.VoteRequest;
using Capstone_VotingSystem.Entities;
using Microsoft.EntityFrameworkCore;

namespace Capstone_VotingSystem.Controller
{
    [Route("api/vote")]
    [ApiController]
    public class VoteController : BaseController
    {
        private readonly IVoteRepositories voteRepositories;
        private readonly VotingSystemContext dbContext;

        public VoteController(IVoteRepositories voteRepositories , VotingSystemContext votingSystemContext)
        {
            this.voteRepositories = voteRepositories;
            this.dbContext = votingSystemContext;
        }
        [HttpGet]
        public async Task<IActionResult> getall()
        {
            return Ok(await dbContext.VotingDetails.ToListAsync());
        }
        [HttpPost]
        public async Task<IActionResult> CreateVote(CreateVoteRequest request)
        {
            try
            {
                if (request == null)
                {
                    return CustomResult("Cu Phap Sai", HttpStatusCode.BadRequest);
                }
                var create = await voteRepositories.CreateVote(request);
                if (create == null)
                {
                    return CustomResult("vote da ton tai", HttpStatusCode.Accepted);
                }
                //var result = _mapper.Map<CreateAccountResponse>(create);
                return CustomResult("tạo vote thành công", create, HttpStatusCode.Created);
            }
            catch (Exception)
            {
                return CustomResult("tạo thất bại", HttpStatusCode.InternalServerError);


            }
        }
    }
}
