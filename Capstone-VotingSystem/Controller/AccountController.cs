using Capstone_VotingSystem.Entities;
using Capstone_VotingSystem.Models.RequestModels.AccountModRequest;
using Capstone_VotingSystem.Repositories.AccountModRepo;
using CoreApiResponse;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Capstone_VotingSystem.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : BaseController
    {
        private readonly IAccountModRepositories accountModRepositories;

        public AccountController(IAccountModRepositories accountRepositories)
        {
            accountModRepositories = accountRepositories;
        }
        [HttpPost("createAccount")]
        public async Task<IActionResult> CreateAccount(CreateAccountModRequest createAccount)
        {
            try
            {
                var result = await accountModRepositories.CreateAccount(createAccount);
                if (result == null)
                {
                    return CustomResult("đã tồn tại", HttpStatusCode.NotFound);
                }
                return CustomResult("Success", result, HttpStatusCode.OK);
            }
            catch (Exception)
            {
                return CustomResult("Fail", HttpStatusCode.InternalServerError);
            }
        }
    }
}
