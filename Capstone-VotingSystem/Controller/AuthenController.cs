using Capstone_VotingSystem.Entities;
using Capstone_VotingSystem.Models.RequestModels.AuthenRequest;
using Capstone_VotingSystem.Repositories.AuthenRepo;
using CoreApiResponse;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace Capstone_VotingSystem.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenController : BaseController
    {
        private readonly VotingSystemContext _votingSystemContext;
        private readonly IAuthenRepositories _authenRepositories;


        public AuthenController(VotingSystemContext votingSystemContext, IAuthenRepositories authenticationRepositories)
        {
            _votingSystemContext = votingSystemContext;
            _authenRepositories = authenticationRepositories;
        }
        [HttpGet("getAccount")]
        public async Task<IActionResult> getAllAccount()
        {
            return Ok(await _votingSystemContext.AccountMods.ToListAsync());
        }
        [HttpPost("Login")]

        public async Task<IActionResult> Login(LoginRequest payLoad)
        {
            try
            {
                var account = await _authenRepositories.SignInAsync(payLoad);
                if (account == null)
                {
                    return CustomResult("Username Or Password wrong!!", HttpStatusCode.NotFound);
                }
                var result = await _authenRepositories.GenerateTokenAsync(account);
                return CustomResult("Success", result, HttpStatusCode.OK);
            }
            catch (Exception)
            {
                return CustomResult("Fail", HttpStatusCode.InternalServerError);

            }

        }
    }
}
