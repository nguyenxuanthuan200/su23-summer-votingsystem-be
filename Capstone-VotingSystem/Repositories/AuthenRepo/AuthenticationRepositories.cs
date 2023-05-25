using Capstone_VotingSystem.Entities;
using Capstone_VotingSystem.Model;
using Capstone_VotingSystem.Model.ResponseModel.AuthenReponse;
using Capstone_VotingSystem.Model.ResquestModel.AuthenRequest;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Capstone_VotingSystem.Repositories.AuthenRepo
{
    public class AuthenticationRepositories : IAuthenticationRepositories
    {
        private readonly VotingSystemContext _votingSystemContext;
        public IConfiguration _configuration;

        public AuthenticationRepositories(VotingSystemContext votingSystemContext, IConfiguration configuration,
            IOptionsMonitor<AppSetting> optionsMonitor) 
        {
            _votingSystemContext = votingSystemContext;
            _configuration = configuration;
        }
        public async Task<LoginReponse> GenerateTokenAsync(Account account)
        {
            var rolename = await _votingSystemContext.Roles.SingleOrDefaultAsync(p => p.RoleId == account.RoleId);
            if (rolename.Name.Equals("admin"))
            {
                var claims = new[]
                {
                    new Claim(ClaimTypes.Role, "Admin"),
                    new Claim(JwtRegisteredClaimNames.Sub,_configuration["JwtConfig:Subject"]),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                    new Claim("Id", account.Id.ToString())
                    };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtConfig:Key"]));

                var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(_configuration["JwtJwtConfig:Issuer"], _configuration["JwtJwtConfig:Audience"], claims, expires: DateTime.UtcNow.AddMinutes(120), signingCredentials: signIn);

                var result = new LoginReponse();
                result.Id = Guid.NewGuid();
                result.token = new JwtSecurityTokenHandler().WriteToken(token);
                result.roleName = rolename.Name;
                return result;
            }
            return null;
        }

        public async Task<AccountMod> SignInAsync(LoginRequest payload)
        {
            var account = await _votingSystemContext.AccountMods.Where(
               p => p.Username == payload.Username && payload.Password == p.Password).SingleOrDefaultAsync();

            if (account == null)
            {
                return null;
            }
            var roleName = await _votingSystemContext.Roles.Where(p => p.RoleId == account.RoleId)
                 .SingleOrDefaultAsync();
            if (roleName.Name.Equals("admin"))
            {
                if (payload.Password == account.Password)

                    return account;
            }
            if (roleName.Name.Equals("manager"))
            {
                if (payload.Password == account.Password)

                    return account;
            }
            if (roleName.Name.Equals("staff"))
            {
                if (payload.Password == account.Password)

                    return account;
            }
            return null;
        }
    }
}

