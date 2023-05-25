using Capstone_VotingSystem.Entities;
using Capstone_VotingSystem.Model;
using Capstone_VotingSystem.Model.ResquestModel.AuthenRequest;
using Capstone_VotingSystem.Repositories.AuthenRepo;
using CoreApiResponse;
using FirebaseAdmin.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Org.BouncyCastle.Asn1.Ocsp;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace Capstone_VotingSystem.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : BaseController
    {
        private readonly VotingSystemContext _votingSystemContext;
        private readonly IAuthenticationRepositories _authenticationRepositories;


        public AuthenticationController(VotingSystemContext votingSystemContext,  IAuthenticationRepositories authenticationRepositories)
        {
            _votingSystemContext = votingSystemContext;
            _authenticationRepositories = authenticationRepositories;
        }
        [HttpGet("getAccount")]
        public async Task<IActionResult> getAllAccount()
        {
            return Ok(await _votingSystemContext.Accounts.ToListAsync());
        }
        [HttpPost("loginGG")]
        public async Task<IActionResult> login(string idtoken)
        {
            FirebaseToken decoded = await FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(idtoken);
            var uid = decoded.Uid;
            UserRecord userrecord = await FirebaseAuth.DefaultInstance.GetUserAsync(uid);

            return Ok(userrecord);
        }
        [HttpPost("Login")]

        public async Task<IActionResult> login(LoginRequest payLoad) 
        {
            try
            {
                var account = await _authenticationRepositories.SignInAsync(payLoad);
                if (account == null)
                {
                    return CustomResult("Username Or Password wrong!!", HttpStatusCode.NotFound);
                }
                var result = await _authenticationRepositories.GenerateTokenAsync(account);
                return CustomResult("Success", result, HttpStatusCode.OK);
            }
            catch (Exception)
            {
                return CustomResult("Fail", HttpStatusCode.InternalServerError);

            }

        }

    }
}
