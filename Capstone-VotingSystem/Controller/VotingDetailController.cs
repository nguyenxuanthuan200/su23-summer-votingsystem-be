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
    public class VotingDetailController : BaseController
    {
        private readonly IVoteRepositories voteRepositories;
     

        public VotingDetailController(IVoteRepositories voteRepositories)
        {
            this.voteRepositories = voteRepositories;
            
        }
        [HttpGet("GetAllVotingDetail")]
        public async Task<IActionResult> getall()
        {
            try
            {
                var result = await voteRepositories.GetAll();
                if (result == null)
                    return CustomResult("Not Found", HttpStatusCode.NotFound);
                return CustomResult("Success", result, HttpStatusCode.OK);
            }
            catch (Exception)
            {
                return CustomResult("Fail", HttpStatusCode.InternalServerError);
            }
        }
        [HttpPost("CreateVotingDetail")]
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
                    return CustomResult("voting không tồn tại", HttpStatusCode.Accepted);
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
