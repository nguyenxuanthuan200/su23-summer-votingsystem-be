using Capstone_VotingSystem.Models.RequestModels.AuthenRequest;
using Capstone_VotingSystem.Services.AuthenticationService;
using CoreApiResponse;
using FirebaseAdmin.Auth;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Capstone_VotingSystem.Controller
{
    [Route("api/v1/authen")]

    [ApiController]
    public class AuthenController : BaseController
    {
        private readonly IAuthenticationService authentiaction;

        public AuthenController(IAuthenticationService authenticationService)
        {
            this.authentiaction = authenticationService;
        }
        [HttpPost("firebase")]
        public async Task<IActionResult> login(string idtoken)
        {
            var result = await authentiaction.LoginFirebase(idtoken);
            return CustomResult("Success", result, HttpStatusCode.OK);
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
