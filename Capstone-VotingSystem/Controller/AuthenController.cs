using Capstone_VotingSystem.Entities;
using Capstone_VotingSystem.Models.RequestModels.AuthenRequest;
using Capstone_VotingSystem.Models.ResponseModels.AuthenResponse;
using Capstone_VotingSystem.Repositories.AuthenRepo;
using CoreApiResponse;
using FirebaseAdmin.Auth;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using Octokit.Internal;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Net;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;

namespace Capstone_VotingSystem.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenController : BaseController
    {
        private static string API_KEY = "AIzaSyDFsJS8u9XsIClfCOGZJQ4vg7JsJFSNA7Q";
        private readonly VotingSystemContext _votingSystemContext;
        private readonly IAuthenRepositories _authenRepositories;
        private readonly IConfiguration _configuration;
      
        public AuthenController(VotingSystemContext votingSystemContext, IAuthenRepositories authenticationRepositories, IConfiguration configuration)
        {
            _votingSystemContext = votingSystemContext;
            _authenRepositories = authenticationRepositories;
            _configuration = configuration;
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
        [HttpPost("Google")]
        public async Task<ActionResult> LoginBygoogle(string idtoken)
        {
            FirebaseToken decoded = await FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(idtoken);
            var uid = decoded.Uid;
            UserRecord userRecord = await FirebaseAuth.DefaultInstance.GetUserAsync(uid);

            return Ok(userRecord);
        }
      
    }
}
