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
        public async Task<APIResponse<UpdateUserResponse>> UpdateUser(string? userId, UpdateUserRequest request)
        {
            APIResponse<UpdateUserResponse> response = new();
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
            response.ToSuccessResponse("Tạo thành công", StatusCodes.Status200OK);
            response.Data = map;
            return response;
        }
    }
}
