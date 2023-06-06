using Capstone_VotingSystem.Entities;
using Capstone_VotingSystem.Models.RequestModels.AuthenRequest;
using Capstone_VotingSystem.Models.ResponseModels.AuthenResponse;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Capstone_VotingSystem.Services.AuthenticationService
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly VotingSystemContext dbContext;
        private readonly IConfiguration _configuration;

        public AuthenticationService(VotingSystemContext votingSystemContext, IConfiguration configuration)
        {
            this.dbContext = votingSystemContext;
            this._configuration = configuration;
        }

        public async Task<ResponseAccount> GenerateToken(Account account)
        {
            var user = await dbContext.Users.SingleOrDefaultAsync(p => p.UserName == account.UserName);
            var roleName = await dbContext.Roles.SingleOrDefaultAsync(p => p.RoleId == user.RoleId);

            if (roleName == null)
            {
                return null;
            }
            if (roleName.Name.Equals("admin"))
            {
                var claims = new[]
                {
                    new Claim(ClaimTypes.Role, "Admin"),
                    new Claim(JwtRegisteredClaimNames.Sub,_configuration["JwtConfig:Subject"]),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                    };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtConfig:Key"]));

                var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(_configuration["JwtConfig:Issuer"], _configuration["JwtConfig:Audience"],
                    claims,
                    expires: DateTime.UtcNow.AddMinutes(120),
                    signingCredentials: signIn);
                var tokenSave = await dbContext.Accounts.SingleOrDefaultAsync(p => p.UserName == account.UserName);

                ResponseAccount result = new ResponseAccount();
                {
                    result.Username = account.UserName;
                    result.Token = new JwtSecurityTokenHandler().WriteToken(token);
                    result.Status = account.Status;
                    result.roleName = roleName.Name;


                }
                tokenSave.Token = result.Token;
                dbContext.Accounts.Update(tokenSave);
                await dbContext.SaveChangesAsync();

                return result;
            }
            if (roleName.Name.Equals("user"))
            {
                var claims = new[]
                {
                    new Claim(ClaimTypes.Role, "User"),
                    new Claim(JwtRegisteredClaimNames.Sub,_configuration["JwtConfig:Subject"]),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                    };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtConfig:Key"]));

                var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(_configuration["JwtConfig:Issuer"], _configuration["JwtConfig:Audience"],
                    claims,
                    expires: DateTime.UtcNow.AddMinutes(120),
                    signingCredentials: signIn);
                var tokenSave = await dbContext.Accounts.SingleOrDefaultAsync(p => p.UserName == account.UserName);

                ResponseAccount result = new ResponseAccount();
                {
                    result.Username = account.UserName;
                    result.Token = new JwtSecurityTokenHandler().WriteToken(token);
                    result.Status = account.Status;
                    result.roleName = roleName.Name;
                }
                tokenSave.Token = result.Token;
                dbContext.Accounts.Update(tokenSave);
                await dbContext.SaveChangesAsync();

                return result;
            }
            return null;
        }

        public async Task<Account> Login(LoginRequest model)
        {
            var acc = await dbContext.Accounts.Where(p => p.UserName == model.UserName && p.Password == model.Password).SingleOrDefaultAsync();
            if (acc != null)
            {
                return acc;
            }
            return null;

        }

        public Task<ResponseLogin> LoginFirebase(LoginFirebaseModel model)
        {
            throw new NotImplementedException();
        }
    }
}
