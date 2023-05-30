using Capstone_VotingSystem.Entities;
using Capstone_VotingSystem.Models.RequestModels.AuthenRequest;
using Capstone_VotingSystem.Models.ResponseModels.AuthenResponse;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Octokit.Internal;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Capstone_VotingSystem.Repositories.AuthenRepo
{
    public class AuthenRepositories : IAuthenRepositories
    {
        private readonly VotingSystemContext dbContext;
        private readonly IConfiguration _configuration;

        public AuthenRepositories(VotingSystemContext votingSystemContext, IConfiguration configuration) 
        {
            dbContext = votingSystemContext;
            _configuration = configuration;
        }
        public async Task<LoginResponse> GenerateTokenAsync(AccountMod accountMod)
        {
            var rolename = await dbContext.Roles.SingleOrDefaultAsync(p => p.RoleId == accountMod.RoleId);
            if (rolename.Name.Equals("admin"))
            {
                var claims = new[]
                {
                    new Claim(ClaimTypes.Role, "Admin"),
                    new Claim(JwtRegisteredClaimNames.Sub,_configuration["JwtConfig:Subject"]),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                    new Claim("Id", accountMod.RoleId.ToString())
                    };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtConfig:Key"]));

                var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(_configuration["JwtJwtConfig:Issuer"], _configuration["JwtJwtConfig:Audience"], 
                    claims, 
                    expires: DateTime.UtcNow.AddMinutes(120), 
                    signingCredentials: signIn);

                var result = new LoginResponse();
                result.Id = Guid.NewGuid();
                result.token = new JwtSecurityTokenHandler().WriteToken(token);
                result.roleName = rolename.Name;
                return result;
            }
            if (rolename.Name.Equals("mod"))
            {
                var claims = new[]
               {
                    new Claim(ClaimTypes.Role, "Mod"),
                    new Claim(JwtRegisteredClaimNames.Sub,_configuration["JwtConfig:Subject"]),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                    new Claim("Id", accountMod.RoleId.ToString()),
                    };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtConfig:Key"]));

                var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(_configuration["JwtJwtConfig:Issuer"], _configuration["JwtJwtConfig:Audience"], claims, expires: DateTime.UtcNow.AddMinutes(120), signingCredentials: signIn);

                var result = new LoginResponse();
                result.Id = Guid.NewGuid();
                result.token = new JwtSecurityTokenHandler().WriteToken(token);
                result.roleName = rolename.Name;
                return result;
            }
            return null;
        }

        public Task<FirebaseResponse<LoginResponse>> Login(LoginFirebaseModel model)
        {
            throw new NotImplementedException();
        }

        public async Task<AccountMod> SignInAsync(LoginRequest payload)
        {
            var account = await dbContext.AccountMods.Where(
               p => p.Username == payload.Username && payload.Password == p.Password).SingleOrDefaultAsync();

            if (account == null)
            {
                return null;
            }
            var roleName = await dbContext.Roles.Where(p => p.RoleId == account.RoleId)
                 .SingleOrDefaultAsync();
            if (roleName.Name.Equals("admin"))
            {
                if (payload.Password == account.Password)

                    return account;
            }
            if (roleName.Name.Equals("mod"))
            {
                if (payload.Password == account.Password)

                    return account;
            }
            return null;
        }

        public Task<ApiResponse<LoginResponse>> SignInAsync(LoginFirebaseModel model)
        {
            throw new NotImplementedException();
        }
    }
}
