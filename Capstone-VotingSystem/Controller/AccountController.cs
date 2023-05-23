using Capstone_VotingSystem.Model;
using Capstone_VotingSystem.Repositories.AccountRepo;
using CoreApiResponse;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using System.Net;

namespace Capstone_VotingSystem.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : BaseController
    {
        private readonly IAccountRepositories _accountRepo;

        public AccountController(IAccountRepositories accountRepositories) { 
            _accountRepo = accountRepositories;
        }
        [HttpPost("createAccount")]
        public async Task<IActionResult> CreateAccount(AccountModel model)
        {
            try
            {
                var result = await _accountRepo.CreateAccount(model);
                if (result == null)
                {
                    return CustomResult("đã tồn tại", HttpStatusCode.NotFound);
                }
                return CustomResult("Success", result, HttpStatusCode.OK);
            }catch (Exception)
            {
                return CustomResult("Fail", HttpStatusCode.InternalServerError);
            }
        }
    }
}
