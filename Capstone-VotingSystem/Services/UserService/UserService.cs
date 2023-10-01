using AutoMapper;
using Capstone_VotingSystem.Core.CoreModel;
using Capstone_VotingSystem.Entities;
using Capstone_VotingSystem.Models.RequestModels.UserRequest;
using Capstone_VotingSystem.Models.ResponseModels.UserResponse;
using CloudinaryDotNet.Actions;
using CloudinaryDotNet;
using Microsoft.EntityFrameworkCore;
using Capstone_VotingSystem.Helpers;
using Microsoft.Extensions.Options;

namespace Capstone_VotingSystem.Services.UserService
{
    public class UserService : IUserService
    {
        private readonly Cloudinary _cloudinary;
        private readonly VotingSystemContext dbContext;
        private readonly IMapper _mapper;

        public UserService(VotingSystemContext votingSystemContext, IMapper mapper, IOptions<CloudinarySettings> config)
        {
            var acc = new CloudinaryDotNet.Account(
            config.Value.CloundName,
            config.Value.ApiKey,
            config.Value.ApiSecret
            );

            _cloudinary = new Cloudinary(acc);
            this.dbContext = votingSystemContext;
            this._mapper = mapper;
        }


        public async Task<APIResponse<ImageUserResponse>> AddImageUserAsync(IFormFile file, string folderName, string? userId)
        {
            APIResponse<ImageUserResponse> response = new();
            var checkUser = await dbContext.Users.Where(p => p.UserId.Equals(userId)).SingleOrDefaultAsync();
            if (checkUser == null)
            {
                response.ToFailedResponse("Người dùng không tồn tại", StatusCodes.Status404NotFound);
                return response;
            }
            var uploadResult = new ImageUploadResult();
            if (file.Length > 0)
            {
                using var stream = file.OpenReadStream();
                var uploadParams = new ImageUploadParams()
                {
                    File = new FileDescription(file.FileName, stream),
                    Transformation = new Transformation().Crop("fill").Gravity("face"),
                    Folder = folderName,

                };

                uploadResult = await _cloudinary.UploadAsync(uploadParams);

            }
            checkUser.AvatarUrl = uploadResult.SecureUrl.AbsoluteUri;

            ImageUserResponse imageRes = new ImageUserResponse();
            {
                imageRes.userId = checkUser.UserId;
                imageRes.AvatarURL = uploadResult.SecureUrl.AbsoluteUri;
                imageRes.PublicId = uploadResult.PublicId;
            }
            dbContext.Users.Update(checkUser);
            await dbContext.SaveChangesAsync();
            response.ToSuccessResponse(imageRes, "Thành công", StatusCodes.Status200OK);
            return response;
        }
        public async Task<APIResponse<IEnumerable<GetListUserResponse>>> GetAllUser()
        {
            APIResponse<IEnumerable<GetListUserResponse>> response = new();
            // List<GetListUserResponse> result = new();
            var users = await dbContext.Users.ToListAsync();
            if (users.Count() == 0)
            {
                response.ToFailedResponse("Không tìm thấy người dùng nào", StatusCodes.Status404NotFound);
                return response;
            }
            var account = await dbContext.Accounts.ToListAsync();
            if (account.Count() == 0)
            {
                response.ToFailedResponse("Không tìm thấy tài khoản nào", StatusCodes.Status404NotFound);
                return response;
            }

            IEnumerable<GetListUserResponse> result = account.Join(users, a => a.UserName, u => u.UserId, (a, u) =>
            {
                var ListPermissionOfUser = checkPermission(u.Permission);
                return new GetListUserResponse()
                {
                    UserName = a.UserName,
                    CreateAt = a.CreateAt,
                    RoleId = a.RoleId,
                    Status = a.Status,
                    FullName = u.FullName,
                    Phone = u.Phone,
                    AvatarUrl = u.AvatarUrl,
                    Address = u.Address,
                    Dob = u.Dob,
                    Email = u.Email,
                    Gender = u.Gender,
                    Permission = ListPermissionOfUser,
                };
            }
                ).ToList();
            if (result.Count() == 0)
            {
                response.ToFailedResponse("Không tìm thấy người dùng nào", StatusCodes.Status400BadRequest);
                return response;
            }
            response.ToSuccessResponse(response.Data = result, "Lấy danh sách thành công", StatusCodes.Status200OK);
            return response;

        }
        private ListPermissionOfUser checkPermission(int a)
        {
            var list = new ListPermissionOfUser();
            if (a == 0)
            {
                list.Voter = true;
                list.Candidate = true;
                list.Moderator = true;
            }
            if (a == 1)
            {
                list.Voter = false;
                list.Candidate = true;
                list.Moderator = true;
            }
            if (a == 2)
            {
                list.Voter = false;
                list.Candidate = false;
                list.Moderator = true;
            }

            if (a == 3)
            {
                list.Voter = true;
                list.Candidate = false;
                list.Moderator = true;
            }
            if (a == 4)
            {
                list.Voter = false;
                list.Candidate = true;
                list.Moderator = false;
            }
            if (a == 5)
            {
                list.Voter = true;
                list.Candidate = true;
                list.Moderator = false;
            }
            if (a == 6)
            {
                list.Voter = true;
                list.Candidate = false;
                list.Moderator = false;
            }
            if (a == 7)
            {
                list.Voter = false;
                list.Candidate = false;
                list.Moderator = false;
            }
            return list;

        }
        public async Task<APIResponse<UpdateUserResponse>> UpdateUser(string? userId, UpdateUserRequest request)
        {
            APIResponse<UpdateUserResponse> response = new();
            var checkAccount = await dbContext.Accounts.Where(p => p.UserName == userId && p.Status == true).SingleOrDefaultAsync();
            if (checkAccount == null)
            {
                response.ToFailedResponse("Không tìm thấy tài khoản", StatusCodes.Status404NotFound);
                return response;
            }
            var checkUser = await dbContext.Users.Where(p => p.UserId == userId && p.Status == true).SingleOrDefaultAsync();
            if (checkUser == null)
            {
                response.ToFailedResponse("Không tìm thấy người dùng", StatusCodes.Status404NotFound);
                return response;
            }

            var uploadResult = new ImageUploadResult();
            if (request.ImageFile != null && request.ImageFile.Length > 0)
            {
                var uploadParams = new ImageUploadParams()
                {
                    File = new FileDescription(request.ImageFile.FileName, request.ImageFile.OpenReadStream()),
                    Transformation = new Transformation().Crop("fill").Gravity("face"),
                };

                uploadResult = await _cloudinary.UploadAsync(uploadParams);

            }
            checkUser.FullName = request.FullName ?? checkUser.FullName;
            checkUser.Phone = request.Phone ?? checkUser.Phone;
            checkUser.AvatarUrl = uploadResult.SecureUrl?.AbsoluteUri ?? checkUser.AvatarUrl;
            checkUser.Gender = request.Gender ?? checkUser.Gender;
            checkUser.Address = request.Address ?? checkUser.Address;
            checkUser.Dob = request.Dob ?? checkUser.Dob;

            dbContext.Users.Update(checkUser);
            await dbContext.SaveChangesAsync();

            var map = _mapper.Map<UpdateUserResponse>(checkUser);
            response.ToSuccessResponse("Cập nhật thành công", StatusCodes.Status200OK);
            response.Data = map;
            return response;
        }

        public async Task<APIResponse<string>> UpdateUserGroup(string userId, Guid groupId, Guid campaignId)
        {
            APIResponse<string> response = new();
            var checkAccount = await dbContext.Accounts.Where(p => p.UserName == userId && p.Status == true).SingleOrDefaultAsync();
            if (checkAccount == null)
            {
                response.ToFailedResponse("Không tìm thấy tài khoản", StatusCodes.Status404NotFound);
                return response;
            }
            var checkUser = await dbContext.Users.Where(p => p.UserId == userId && p.Status == true).SingleOrDefaultAsync();
            if (checkUser == null)
            {
                response.ToFailedResponse("Không tìm thấy người dùng", StatusCodes.Status404NotFound);
                return response;
            }
            var checkGroupRequest = await dbContext.Groups.Where(p => p.GroupId == groupId && p.CampaignId == campaignId).SingleOrDefaultAsync();
            if (checkGroupRequest == null)
            {
                response.ToFailedResponse("Không tìm thấy nhóm", StatusCodes.Status404NotFound);
                return response;
            }
            var checkGroup = await dbContext.GroupUsers.Where(p => p.UserId == userId && p.CampaignId == campaignId).ToListAsync();
            foreach (var item in checkGroup)
            {
                var checkGroupVoter = await dbContext.Groups.Where(p => p.GroupId == item.GroupId).SingleOrDefaultAsync();
                if (checkGroupVoter.IsVoter == checkGroupRequest.IsVoter && checkGroupVoter.IsStudentMajor == checkGroupRequest.IsStudentMajor)
                {
                    item.GroupId = groupId;
                    dbContext.Update(item);
                    await dbContext.SaveChangesAsync();
                    response.ToSuccessResponse("Chọn nhóm thành công", StatusCodes.Status200OK);
                    return response;
                }
            }
            var id = Guid.NewGuid();
            GroupUser groupUser = new GroupUser();
            {
                groupUser.GroupUserId = id;
                groupUser.GroupId = groupId;
                groupUser.UserId = userId;
                groupUser.CampaignId = campaignId;
            }
            await dbContext.GroupUsers.AddAsync(groupUser);
            await dbContext.SaveChangesAsync();
            response.ToSuccessResponse("Chọn nhóm thành công", StatusCodes.Status200OK);
            return response;
        }

        public async Task<APIResponse<GetListUserResponse>> UpdateUserPermission(string userId, UserPermissionRequest request)
        {
            APIResponse<GetListUserResponse> response = new();
            // List<GetListUserResponse> result = new();
            var users = await dbContext.Users.Where(p => p.Status == true && p.UserId == userId).SingleOrDefaultAsync();
            if (users == null)
            {
                response.ToFailedResponse("Không tìm thấy người dùng nào", StatusCodes.Status404NotFound);
                return response;
            }
            var account = await dbContext.Accounts.Where(p => p.Status == true && p.UserName == userId).SingleOrDefaultAsync();
            if (account == null)
            {
                response.ToFailedResponse("Không tìm thấy tài khoản nào", StatusCodes.Status404NotFound);
                return response;
            }
            var checkPer = checkPermissionNumber(request.Voter, request.Candidate, request.Moderator);

            users.Permission = checkPer;
            dbContext.Users.Update(users);
            await dbContext.SaveChangesAsync();
            response.ToSuccessResponse("Cập nhật thành công", StatusCodes.Status200OK);
            return response;
        }
        private int checkPermissionNumber(bool voter, bool candidate, bool moderator)
        {
            int a = 0;
            if (voter == true && candidate == true && moderator == true) return a = 0;
            if (voter == false && candidate == true && moderator == true) return a = 1;
            if (voter == false && candidate == false && moderator == true) return a = 2;
            if (voter == true && candidate == false && moderator == true) return a = 3;
            if (voter == false && candidate == true && moderator == false) return a = 4;
            if (voter == true && candidate == true && moderator == false) return a = 5;
            if (voter == true && candidate == false && moderator == false) return a = 6;
            if (voter == false && candidate == false && moderator == false) return a = 7;


            return 0;
        }

        public async Task<APIResponse<GetUserByIdResponse>> GetUserById(string id)
        {
            APIResponse<GetUserByIdResponse> response = new();
            // List<GetListUserResponse> result = new();
            var users = await dbContext.Users.Where(p => p.Status == true && p.UserId == id).SingleOrDefaultAsync();
            if (users == null)
            {
                response.ToFailedResponse("Không tìm thấy người dùng hoặc người dùng đã bị xóa", StatusCodes.Status404NotFound);
                return response;
            }
            var account = await dbContext.Accounts.Where(p => p.Status == true && p.UserName == id).SingleOrDefaultAsync();
            if (account == null)
            {
                response.ToFailedResponse("Không tìm thấy tài khoản nào hoặc đã bị xóa", StatusCodes.Status404NotFound);
                return response;
            }
            var map = _mapper.Map<GetUserByIdResponse>(users);


            response.ToSuccessResponse(response.Data = map, "Lấy chi tiết người dùng thành công", StatusCodes.Status200OK);
            return response;
        }
    }
}

