using CoreApiResponse;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Capstone_VotingSystem.Repositories.VoteRepo;
using System.Net;
using Capstone_VotingSystem.Models.RequestModels.VoteRequest;

namespace Capstone_VotingSystem.Controller
{
    [Route("api/vote")]
    [ApiController]
    public class VoteController : BaseController
    {
        private readonly IVoteRepositories voteRepositories;
        public VoteController(IVoteRepositories voteRepositories)
        {
            this.voteRepositories = voteRepositories;
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
                return CustomResult("Success", create, HttpStatusCode.Created);
            }
            catch (Exception)
            {
                return CustomResult("Fail", HttpStatusCode.InternalServerError);


            }
        }
    }
}
