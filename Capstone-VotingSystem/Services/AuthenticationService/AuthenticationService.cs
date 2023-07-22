using Capstone_VotingSystem.Entities;
using Capstone_VotingSystem.Models.RequestModels.AuthenRequest;
using Capstone_VotingSystem.Models.ResponseModels.AuthenResponse;
using FirebaseAdmin.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Data;
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
            //var user = await dbContext.Users.SingleOrDefaultAsync(p => p.UserId == account.UserName);
            var roleName = await dbContext.Roles.SingleOrDefaultAsync(p => p.RoleId == account.RoleId);

            if (roleName == null)
            {
                return null;
            }
            if (roleName.Name.Equals("admin"))
            {
                var claims = new[]
                {
                    new Claim(ClaimTypes.Role, "Admin"),
                    new Claim("Username", account.UserName),
                    new Claim("RoleId", roleName.RoleId.ToString()),
                    new Claim("RoleName", roleName.Name),
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

                    result.Token = new JwtSecurityTokenHandler().WriteToken(token);



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
                    new Claim("Username", account.UserName),
                    new Claim("RoleId", roleName.RoleId.ToString()),
                    new Claim("RoleName", roleName.Name),
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
                    result.Token = new JwtSecurityTokenHandler().WriteToken(token);
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

        public async Task<ResponseFireBase> LoginFirebase(string idToken)
        {
            FirebaseToken decoded = await FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(idToken);
            var uid = decoded.Uid;
            UserRecord userrecord = await FirebaseAuth.DefaultInstance.GetUserAsync(uid);

            var check = await dbContext.Users.Where(p => p.UserId == userrecord.Email).SingleOrDefaultAsync();
            var role = await dbContext.Roles.Where(p => p.Name.Equals("user")).SingleOrDefaultAsync();
            if (check == null)
            {

                User user = new User();
                {
                    user.UserId = userrecord.Email;
                    user.FullName = userrecord.DisplayName;
                    user.AvatarUrl = userrecord.PhotoUrl;
                    user.Email = userrecord.Email;
                }
                var claims = new[]
                   {
                    new Claim(ClaimTypes.Role, "User"),
                    new Claim("RoleId", role.RoleId.ToString()),
                    new Claim("userId", user.UserId),
                    new Claim("RoleName", role.Name),
                    new Claim("Photo", userrecord.PhotoUrl),

                    };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtConfig:Key"]));

                var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(_configuration["JwtConfig:Issuer"], _configuration["JwtConfig:Audience"],
                    claims,
                    expires: DateTime.UtcNow.AddMinutes(120),
                    signingCredentials: signIn);
                Account account = new Account();
                {
                    account.UserName = userrecord.Email;
                    account.Token = new JwtSecurityTokenHandler().WriteToken(token);
                    account.Status = userrecord.EmailVerified;
                    account.RoleId = role.RoleId;
                    account.CreateAt = DateTime.UtcNow;
                }
                await dbContext.Users.AddAsync(user);
                await dbContext.Accounts.AddAsync(account);
                await dbContext.SaveChangesAsync();
                ResponseFireBase res = new ResponseFireBase();
                {
                    res.Token = account.Token;

                }
                return res;
            }
            if (check != null)
            {
                var status = await dbContext.Accounts.Where(p => p.Status.Equals("True").Equals("False")).FirstOrDefaultAsync();

                if (true)
                {
                    var claims = new[]
                   {
                    //new Claim(ClaimTypes.Role, "User"),
                   new Claim(ClaimTypes.Role, "User"),
                    new Claim("RoleId", role.RoleId.ToString()),
                    new Claim("RoleName", role.Name),
                    new Claim("Username", status.UserName),
                    new Claim("Email", userrecord.Email),
                    new Claim("Photo", userrecord.PhotoUrl),
                    };

                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtConfig:Key"]));

                    var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                    var token = new JwtSecurityToken(_configuration["JwtConfig:Issuer"], _configuration["JwtConfig:Audience"],
                        claims,
                        expires: DateTime.UtcNow.AddMinutes(120),
                        signingCredentials: signIn);
                    var account = await dbContext.Accounts.SingleOrDefaultAsync(p => p.UserName == userrecord.Email);
                    account.Token = new JwtSecurityTokenHandler().WriteToken(token);

                    dbContext.Accounts.Update(account);
                    await dbContext.SaveChangesAsync();
                    ResponseFireBase res = new ResponseFireBase();
                    {
                        res.Token = account.Token;
                    }
                    return res;
                }

            }
            return null;
        }
    }
}
