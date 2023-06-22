using Capstone_VotingSystem.Core.CoreModel;
using Capstone_VotingSystem.Entities;
using Capstone_VotingSystem.Models.ResponseModels.AccountResponse;
using Microsoft.EntityFrameworkCore;

namespace Capstone_VotingSystem.Services.AccountService
{
    public class AccountService : IAccountService
    {
        private readonly VotingSystemContext dbContext;

        public AccountService(VotingSystemContext votingSystemContext)
        {
            this.dbContext = votingSystemContext;
        }

        public async Task<APIResponse<string>> BanAccount(string id)
        {
            APIResponse<string> response = new();
            var account = await dbContext.Accounts.Where(p =>p.UserName==id&& p.Status == true).SingleOrDefaultAsync();
            if(account==null)
            {
                response.ToFailedResponse("account không tồn tại hoặc đã bị ban", StatusCodes.Status400BadRequest);
                return response;
            }
            var user = await dbContext.Users.Where(p => p.UserId == id && p.Status == true).SingleOrDefaultAsync();
            if (user == null)
            {
                response.ToFailedResponse("account không tồn tại hoặc đã bị ban", StatusCodes.Status400BadRequest);
                return response;
            }           
            account.Status = false;
            user.Status = false;
            dbContext.Accounts.Update(account);
            await dbContext.SaveChangesAsync();
            dbContext.Users.Update(user);
            await dbContext.SaveChangesAsync();
            response.ToSuccessResponse("Ban account thành công", StatusCodes.Status200OK);
            return response;
        }

        public async Task<APIResponse<IEnumerable<AccountResponse>>> GetAllAcount()
        {
            APIResponse<IEnumerable<AccountResponse>> response = new();
            var account = await dbContext.Accounts.Where(p => p.Status == true).ToListAsync();
            if (account == null)
            {
                response.ToFailedResponse("không thấy", StatusCodes.Status400BadRequest);
                return response;
            }
            IEnumerable<AccountResponse> result = account.Select(
                x =>
                {
                    return new AccountResponse()
                    {
                        UserName = x.UserName,
                        CreateAt = DateTime.Now,
                        RoleId = x.RoleId,

                    };
                }
                ).ToList();
            if (result == null)
            {
                response.ToFailedResponse("không thấy", StatusCodes.Status400BadRequest);
                return response;
            }
            response.ToSuccessResponse(response.Data = result, "Lấy danh sách thành công", StatusCodes.Status200OK);
            return response;

        }
    }
}
