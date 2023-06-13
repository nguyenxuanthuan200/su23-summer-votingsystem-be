using CoreApiResponse;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Capstone_VotingSystem.Models.RequestModels.VoteRequest;
using Capstone_VotingSystem.Models.RequestModels.VoteDetailRequest;
using Capstone_VotingSystem.Services.VoteService;
using Microsoft.AspNetCore.Authorization;

namespace Capstone_VotingSystem.Controller
{
    [Route("api/vote")]
    [ApiController]
    public class VoteController : BaseController
    {
        private readonly IVoteService voteService;
        public VoteController(IVoteService voteService)
        {
            this.voteService = voteService;
        }
        [Authorize(Roles = "User")]
        [HttpPost]
        public async Task<IActionResult> CreateVote(CreateVoteRequest request)
        {
            try
            {
                if (request == null)
                {
                    return CustomResult("Cu Phap Sai", HttpStatusCode.BadRequest);
                }
                var create = await voteService.CreateVote(request);
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
        [Authorize(Roles = "User")]
        [HttpPost("votedetail")]
        public async Task<IActionResult> CreateVoteDetail(CreateVoteDetailRequest request)
        {
            try
            {
                if (request == null)
                {
                    return CustomResult("Cu Phap Sai", HttpStatusCode.BadRequest);
                }
                var create = await voteService.CreateVoteDetail(request);
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
