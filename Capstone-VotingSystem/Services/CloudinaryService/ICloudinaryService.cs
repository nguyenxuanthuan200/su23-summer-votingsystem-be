using Capstone_VotingSystem.Core.CoreModel;
using Capstone_VotingSystem.Models.ResponseModels.ImageResponse;
using CloudinaryDotNet.Actions;

namespace Capstone_VotingSystem.Services.CloudinaryService
{
    public interface ICloudinaryService
    {
        Task<ImageUploadResponse> AddImageAsync(IFormFile file, string folderName);
        public Task<DeletionResult> DeleteImage(string publicId);
    }
}
