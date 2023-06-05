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
        private readonly IAuthenticationService authentiaction;

        public AuthenController(IAuthenticationService authenticationService)
        {
            this.authentiaction = authenticationService;
        }
        [HttpPost("Firebase")]
        public async Task<IActionResult> login(string idtoken)
        {
            FirebaseToken decoded = await FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(idtoken);
            var uid = decoded.Uid;
            UserRecord userrecord = await FirebaseAuth.DefaultInstance.GetUserAsync(uid);

            return Ok();
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            try
            {
                var account = await authentiaction.Login(request);
                if (account == null)
                {
                    return CustomResult("Username Or Password wrong!!", HttpStatusCode.NotFound);
                }

                var result = await authentiaction.GenerateToken(account);
                return CustomResult("Success", result, HttpStatusCode.OK);
            }
            catch (Exception)
            {
                return CustomResult("Fail", HttpStatusCode.InternalServerError);

            }
        }

    }
}
