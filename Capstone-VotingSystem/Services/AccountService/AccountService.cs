using Capstone_VotingSystem.Core.CoreModel;
using Capstone_VotingSystem.Entities;
using Capstone_VotingSystem.Models.RequestModels.AccountRequest;
using Capstone_VotingSystem.Models.ResponseModels.AccountResponse;
using Capstone_VotingSystem.Models.ResponseModels.ActionHistoryResponse;
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
            var account = await dbContext.Accounts.Where(p => p.UserName == id && p.Status == true).SingleOrDefaultAsync();
            if (account == null)
            {
                response.ToFailedResponse("Tài khoản không tồn tại hoặc đã bị cấm", StatusCodes.Status400BadRequest);
                return response;
            }
            var user = await dbContext.Users.Where(p => p.UserId == id && p.Status == true).SingleOrDefaultAsync();
            if (user == null)
            {
                response.ToFailedResponse("Tài khoản không tồn tại hoặc đã bị cấm", StatusCodes.Status400BadRequest);
                return response;
            }
            account.Status = false;
            user.Status = false;
            dbContext.Accounts.Update(account);
            dbContext.Users.Update(user);
            await dbContext.SaveChangesAsync();
            response.ToSuccessResponse("Cấm tài khoản thành công", StatusCodes.Status200OK);
            return response;
        }

        public async Task<APIResponse<string>> CreateAccount(CreateAccountRequest request)
        {
            APIResponse<string> response = new();
            var checkAccount = await dbContext.Accounts.Where(p => p.UserName == request.UserName).SingleOrDefaultAsync();
            if (checkAccount != null)
            {
                response.ToFailedResponse("Tên tài khoản đã tồn tại. Vui lòng chọn tên tài khoản khác", StatusCodes.Status400BadRequest);
                return response;
            }
            if (request.Password != request.RePassword)
            {
                response.ToFailedResponse("Mật khẩu nhập lại không trùng khớp với mật khẩu. Vui lòng kiểm tra và thử lại !!", StatusCodes.Status400BadRequest);
                return response;
            }
            var getRole = await dbContext.Roles.Where(p => p.Name.ToUpper().Equals(request.RoleName.ToUpper())).SingleOrDefaultAsync();
            if (getRole == null)
            {
                response.ToFailedResponse("Vai trò không tồn tại. Vui lòng kiểm tra và thử lại !!", StatusCodes.Status400BadRequest);
                return response;
            }

            TimeZoneInfo vnTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
            DateTime currentDateTimeVn = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, vnTimeZone);
            Account acc = new Account();
            {
                acc.UserName = request.UserName;
                acc.Password = request.Password;
                acc.CreateAt = currentDateTimeVn;
                acc.Status = true;
                acc.RoleId = getRole.RoleId;
            }
            User us = new User();
            {
                us.UserId = request.UserName;
                us.FullName = "";
                us.Status = true;
                us.Permission = 0;
            }
            await dbContext.Users.AddAsync(us);
            await dbContext.Accounts.AddAsync(acc);
            await dbContext.SaveChangesAsync();
            response.ToSuccessResponse("Tạo tài khoản thành công", StatusCodes.Status200OK);
            return response;

        }

        //public async Task<APIResponse<IEnumerable<AccountResponse>>> GetAllAcount()
        //{
        //    APIResponse<IEnumerable<AccountResponse>> response = new();
        //    var account = await dbContext.Accounts.Where(p => p.Status == true).ToListAsync();
        //    if (account == null)
        //    {
        //        response.ToFailedResponse("không thấy tài khoản", StatusCodes.Status400BadRequest);
        //        return response;
        //    }
        //    var users = await dbContext.Users.Where(p => p.Status == true).ToListAsync();
        //    if (users == null)
        //    {
        //        response.ToFailedResponse("Không tìm thấy thông tin người dùng", StatusCodes.Status404NotFound);
        //        return response;
        //    }

        //    IEnumerable<AccountResponse> result = account.Join(users, a => a.UserName, u => u.UserId, (a, u) =>
        //        {
        //            return new AccountResponse()
        //            {
        //                UserName = a.UserName,
        //                CreateAt = a.CreateAt,
        //                RoleId = a.RoleId,
        //                Status = a.Status,
        //                FullName = u.FullName,
        //                Phone = u.Phone,
        //                AvatarUrl = u.AvatarUrl,
        //                StatusUser = u.Status,
        //                Address = u.Address,
        //                Dob = u.Dob,
        //                Email = u.Email,
        //                Gender = u.Gender,
        //            };
        //        }
        //        ).ToList();
        //    if (result == null)
        //    {
        //        response.ToFailedResponse("không thấy", StatusCodes.Status400BadRequest);
        //        return response;
        //    }
        //    response.ToSuccessResponse(response.Data = result, "Lấy danh sách thành công", StatusCodes.Status200OK);
        //    return response;

        //}

        public async Task<APIResponse<IEnumerable<AccountResponse>>> GetUsername(string? username)
        {
            APIResponse<IEnumerable<AccountResponse>> response = new();
            var checkUserName = await dbContext.Accounts.Where(p => p.UserName == username).SingleOrDefaultAsync();
            if (checkUserName == null)
            {
                response.ToFailedResponse("Người dùng không tồn tại.", StatusCodes.Status404NotFound);
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
                response.ToFailedResponse("Không có ứng cử viên nào trong chiến dịch", StatusCodes.Status400BadRequest);
                return response;
            }
            response.ToSuccessResponse(response.Data = result, "Lấy danh sách thành công", StatusCodes.Status200OK);
            return response;
        }
        public async Task<APIResponse<string>> UnbanAccount(string id)
        {
            APIResponse<string> response = new();
            var account = await dbContext.Accounts.Where(p => p.UserName == id && p.Status == false).SingleOrDefaultAsync();
            if (account == null)
            {
                response.ToFailedResponse("Tài khoản không bị cấm hoặc không tồn tại", StatusCodes.Status400BadRequest);
                return response;
            }
            var user = await dbContext.Users.Where(p => p.UserId == id && p.Status == false).SingleOrDefaultAsync();
            if (user == null)
            {
                response.ToFailedResponse("Tài khoản không bị cấm hoặc không tồn tại", StatusCodes.Status400BadRequest);
                return response;
            }
            account.Status = true;
            user.Status = true;
            dbContext.Accounts.Update(account);
            dbContext.Users.Update(user);
            await dbContext.SaveChangesAsync();
            response.ToSuccessResponse("Gỡ cấm tài khoản thành công", StatusCodes.Status200OK);
            return response;
        }


    }
}
