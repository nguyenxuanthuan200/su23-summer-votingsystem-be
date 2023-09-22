using Capstone_VotingSystem.Controllers;
using Capstone_VotingSystem.Models.RequestModels.AccountRequest;
using Capstone_VotingSystem.Services.AccountService;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Capstone_VotingSystem.Controller
{
    [Route("api/v1/accounts")]
    [ApiController]
    public class AccountController : BaseApiController
    {
        private readonly IAccountService account;

        public AccountController(IAccountService accountService)
        {
            this.account = accountService;
        }
       // [Authorize(Roles = "Admin")]
        [HttpPost]
        [SwaggerOperation(summary: "Create account for admin")]
        public async Task<IActionResult> CreateAccount(CreateAccountRequest request)
        {
            try
            {
                var result = await account.CreateAccount(request);
                if (result.Success == false)
                {
                    return BadRequest(result);
                }
                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from the database.");
            }
        }
        // [Authorize(Roles = "Admin")]
        [HttpDelete("{userid}")]
        [SwaggerOperation(summary: "Ban Account")]
        public async Task<IActionResult> BanAccount(string userid)
        {
            try
            {
                var result = await account.BanAccount(userid);
                if (result.Success == false)
                {
                    return BadRequest(result);

                }
                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from the database.");
            }
        }
        // [Authorize(Roles = "Admin")]
        [HttpPut("{userid}")]
        [SwaggerOperation(summary: "UnBan Account")]
        public async Task<IActionResult> UnbanAccount(string userid)
        {
            try
            {
                var result = await account.UnbanAccount(userid);
                if (result.Success == false)
                {
                    return BadRequest(result);

                }
                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from the database.");
            }
        }
    }
}
