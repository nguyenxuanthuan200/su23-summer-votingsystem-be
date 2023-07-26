using Capstone_VotingSystem.Core.CoreModel;
using Capstone_VotingSystem.Entities;
using Capstone_VotingSystem.Models.ResponseModels.AccountResponse;
using Capstone_VotingSystem.Models.ResponseModels.ActionHistoryResponse;
using Microsoft.EntityFrameworkCore;
using Octokit;

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
            var account = await dbContext.Accounts.Where(p => p.UserName == id && p.Status == true).SingleOrDefaultAsync();
            if (account == null)
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
                response.ToFailedResponse("không thấy tài khoản", StatusCodes.Status400BadRequest);
                return response;
            }
            var users = await dbContext.Users.Where(p => p.Status == true).ToListAsync();
            if (users == null)
            {
                response.ToFailedResponse("Không tìm thấy thông tin người dùng", StatusCodes.Status404NotFound);
                return response;
            }

            IEnumerable<AccountResponse> result = account.Join(users, a => a.UserName, u => u.UserId, (a, u) =>
                {
                    return new AccountResponse()
                    {
                        UserName = a.UserName,
                        CreateAt = a.CreateAt,
                        RoleId = a.RoleId,
                        Status = a.Status,
                        FullName = u.FullName,
                        Phone = u.Phone,
                        AvatarUrl = u.AvatarUrl,
                        StatusUser = u.Status,
                        Address = u.Address,
                        Dob = u.Dob,
                        Email = u.Email,
                        Gender = u.Gender,
                        GroupId = u.GroupId,
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

        public async Task<APIResponse<IEnumerable<AccountResponse>>> GetUsername(string? username)
        {
            APIResponse<IEnumerable<AccountResponse>> response = new();
            var checkUserName = await dbContext.Accounts.Where(p => p.UserName == username).SingleOrDefaultAsync();
            if (checkUserName == null)
            {
                response.ToFailedResponse("không tồn tại user", StatusCodes.Status404NotFound);
                return response;
            }
            var acc = await dbContext.Accounts.Where(p => p.UserName == username).ToListAsync();
            List<AccountResponse> result = new List<AccountResponse>();
            foreach (var item in acc)
            {
                //var roleName = await dbContext.Roles.SingleOrDefaultAsync(p => p.RoleId == checkUserName.RoleId);
                var actions = new AccountResponse();
                {
                    actions.UserName = item.UserName;
                    actions.CreateAt = item.CreateAt;
                    actions.Status = item.Status;
                    actions.RoleId = item.RoleId;
                    //actions.RoleName = roleName.Name;
                }
                result.Add(actions);
            }
            if (result == null)
            {
                response.ToFailedResponse("Không có Candidate nào trong Campaign", StatusCodes.Status400BadRequest);
                return response;
            }
            response.ToSuccessResponse(response.Data = result, "Lấy danh sách thành công", StatusCodes.Status200OK);
            return response;
        }
    }
}
