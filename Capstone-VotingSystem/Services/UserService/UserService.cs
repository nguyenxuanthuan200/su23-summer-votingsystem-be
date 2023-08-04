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
                response.ToFailedResponse("user không tồn tại", StatusCodes.Status404NotFound);
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
            response.ToSuccessResponse(imageRes, "ok", StatusCodes.Status200OK);
            return response;
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
                response.ToFailedResponse("không tìm thấy người dùng", StatusCodes.Status404NotFound);
                return response;
            }

            var uploadResult = new ImageUploadResult();
            if (request.ImageFile.Length > 0)
            {
                var uploadParams = new ImageUploadParams()
                {
                    File = new FileDescription(request.ImageFile.FileName, request.ImageFile.OpenReadStream()),
                    Transformation = new Transformation().Crop("fill").Gravity("face"),
                };

                uploadResult = await _cloudinary.UploadAsync(uploadParams);

            }
            checkUser.FullName = request.FullName;
            checkUser.Phone = request.Phone;
            checkUser.AvatarUrl = uploadResult.SecureUrl.AbsoluteUri;
            checkUser.Gender = request.Gender;
            checkUser.Address = request.Address;
            checkUser.Dob = request.Dob;

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
                response.ToFailedResponse("không tìm thấy người dùng", StatusCodes.Status404NotFound);
                return response;
            }
            var checkGroup = await dbContext.GroupUsers.Where(p => p.UserId == userId && p.GroupId == groupId && p.CampaignId == campaignId).SingleOrDefaultAsync();
            if (checkGroup == null)
            {
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
                response.ToSuccessResponse("Cập nhật thành công", StatusCodes.Status200OK);
                return response;
            }
            checkGroup.GroupId = groupId;
            dbContext.Update(checkGroup);
            await dbContext.SaveChangesAsync();
            response.ToSuccessResponse("Cập nhật thành công", StatusCodes.Status200OK);
            return response;
        }
    }
}
