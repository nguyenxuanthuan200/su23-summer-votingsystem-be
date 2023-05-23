using Capstone_VotingSystem.Entities;
using Capstone_VotingSystem.Model;
using CoreApiResponse;
using FirebaseAdmin.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Capstone_VotingSystem.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : BaseController
    {
        private readonly VotingSystemContext _votingSystemContext;
        private readonly AppSetting _appSettings;

        public AuthenticationController(VotingSystemContext votingSystemContext, IOptionsMonitor<AppSetting> optionsMonitor)
        {
            _votingSystemContext = votingSystemContext;
            _appSettings = optionsMonitor.CurrentValue;
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
        public IActionResult Validate(LoginModel model)
        {
            var user = _votingSystemContext.Accounts.SingleOrDefault(
                p => p.Username == model.UserName && model.Password == p.Password);
            
            if (user == null)
            {
                return Ok(new APIResponse
                {
                    Success = false,
                    Message = "Invalid Username or password"
                });
            }
            //cấp token
            return Ok(new APIResponse
            {
                Success = true,
                Message = "Authentication Success",
                Data = GenerateTokenAsync(user)
            });
        }

        private  string GenerateTokenAsync(Account account)
        {
           
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var secretKeybytes = Encoding.UTF8.GetBytes(_appSettings.SecretKey);
            var tokenDescription = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    
                    new Claim("Id", account.Id.ToString()),
                    new Claim("Username", account.Username),
                    new Claim("TokenId", Guid.NewGuid().ToString())


                    //role

                }),
                Expires = DateTime.UtcNow.AddHours(4),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKeybytes),
                SecurityAlgorithms.HmacSha256Signature)

            };
            var token = jwtTokenHandler.CreateToken(tokenDescription);
            return jwtTokenHandler.WriteToken(token);

        }

    }
}
