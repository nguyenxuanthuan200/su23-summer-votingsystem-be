using Capstone_VotingSystem.Core.CoreModel;
using Capstone_VotingSystem.Models.ResponseModels.ImageResponse;
using CloudinaryDotNet.Actions;

namespace Capstone_VotingSystem.Services.CloudinaryService
{
    public interface ICloudinaryService
    {
        public Task<APIResponse<ImageUploadResponse>> AddImageUserAsync(IFormFile file, string folderName, string? userId);
        public Task<APIResponse<ImageCampaignResponse>> AddImageCampaignAsync(IFormFile file, string folderName, Guid? campaignId);
    }
}
