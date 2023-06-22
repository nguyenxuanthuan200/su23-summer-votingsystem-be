using Capstone_VotingSystem.Helpers;
using Capstone_VotingSystem.Models.ResponseModels.ImageResponse;
using CloudinaryDotNet.Actions;
using CloudinaryDotNet;
using Microsoft.Extensions.Options;
using Capstone_VotingSystem.Core.CoreModel;
using Capstone_VotingSystem.Entities;
using Microsoft.EntityFrameworkCore;
using Octokit;

namespace Capstone_VotingSystem.Services.CloudinaryService
{
    public class CloudinaryService : ICloudinaryService
    {
        private readonly Cloudinary _cloudinary;
        private readonly VotingSystemContext _dbContext;

        public CloudinaryService(IOptions<CloudinarySettings> config, VotingSystemContext votingSystem)
        {
            var acc = new CloudinaryDotNet.Account(
            config.Value.CloundName,
            config.Value.ApiKey,
            config.Value.ApiSecret
        );

            _cloudinary = new Cloudinary(acc);
            _dbContext = votingSystem;
        }

        public async Task<APIResponse<ImageCampaignResponse>> AddImageCampaignAsync(IFormFile file, string folderName, Guid? campaignId)
        {
            APIResponse<ImageCampaignResponse> response = new();
            var checkCampaign = await _dbContext.Campaigns.Where(p => p.CampaignId.Equals(campaignId)).SingleOrDefaultAsync();
            if (checkCampaign == null)
            {
                response.ToFailedResponse("Campaign không tồn tại", StatusCodes.Status404NotFound);
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
            checkCampaign.ImgUrl = uploadResult.SecureUrl.AbsoluteUri;

            ImageCampaignResponse imageRes = new ImageCampaignResponse();
            {
                imageRes.CampaignId = checkCampaign.CampaignId;
                imageRes.SecureUrl = uploadResult.SecureUrl.AbsoluteUri;
                imageRes.PublicId = uploadResult.PublicId;
            }
            _dbContext.Campaigns.Update(checkCampaign);
            await _dbContext.SaveChangesAsync();
            response.ToSuccessResponse(imageRes, "ok", StatusCodes.Status200OK);
            return response;
        }

        public async Task<APIResponse<ImageUploadResponse>> AddImageUserAsync(IFormFile file, string folderName, string? userId)
        {
            APIResponse<ImageUploadResponse> response = new();
            var checkUser = await _dbContext.Users.Where(p => p.UserId.Equals(userId)).SingleOrDefaultAsync();
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

            ImageUploadResponse imageRes = new ImageUploadResponse();
            {
                imageRes.userId = checkUser.UserId;
                imageRes.SecureUrl = uploadResult.SecureUrl.AbsoluteUri;
                imageRes.PublicId = uploadResult.PublicId;
            }
            _dbContext.Users.Update(checkUser);
            await _dbContext.SaveChangesAsync();
            response.ToSuccessResponse(imageRes, "ok", StatusCodes.Status200OK);
            return response;
        }

    }
}
