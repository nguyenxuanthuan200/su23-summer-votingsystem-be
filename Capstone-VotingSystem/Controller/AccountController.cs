using Capstone_VotingSystem.Controllers;
using Capstone_VotingSystem.Services.AccountService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
       // [Authorize(Roles = "User,Admin")]
        [HttpGet]
        [SwaggerOperation(summary: "Get All Account by Role is Admin and User")]
        public async Task<IActionResult> GetAllAccount()
        {
            try
            {
                var result = await account.GetAllAcount();
                if (result.Success == false)
                {
                    return BadRequest(result.Message);
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
        [HttpDelete("{id}")]
        [SwaggerOperation(summary: "Ban Account")]
        public async Task<IActionResult> BanAccount(string id)
        {
            try
            {
                var result = await account.BanAccount(id);
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
