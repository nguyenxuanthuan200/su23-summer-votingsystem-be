using Capstone_VotingSystem.Entities;
using Capstone_VotingSystem.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Nest;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Capstone_VotingSystem.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly VotingSystemContext _votingSystemContext;
        private readonly AppSetting _appSettings;

        public AuthenticationController(VotingSystemContext votingSystemContext, IOptionsMonitor<AppSetting> optionsMonitor)
        {
            _votingSystemContext = votingSystemContext;
            _appSettings = optionsMonitor.CurrentValue;
        }
        [HttpGet]
        public async Task<IActionResult> getAllAccount()
        {
            return Ok(await _votingSystemContext.Accounts.ToListAsync());
        }
        [HttpPost("Login")]
        public IActionResult validate(LoginModel model)
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
                Data = GenerateToken(user)
            });
        }

        private string GenerateToken(Account account)
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

                }),
                Expires = DateTime.UtcNow.AddMinutes(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKeybytes),
                SecurityAlgorithms.HmacSha256Signature)

            };
            var token = jwtTokenHandler.CreateToken(tokenDescription);
            return jwtTokenHandler.WriteToken(token);


            //return new TokenModel
            //{
            //    AccessToken = accessToken,
            //    RefeshToken = GenerateRefeshToken(),
            //};

        }

        //private string GenerateRefeshToken()
        //{
        //    var random = new byte[32];
        //    using (var raccount =RandomNumberGenerator.Create()) 
        //    {
        //        raccount.GetBytes(random);
        //        return Convert.ToBase64String(random);
        //    }
        //}
    }
}
