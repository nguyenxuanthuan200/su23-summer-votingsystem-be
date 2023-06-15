using Capstone_VotingSystem.Helpers;
using Capstone_VotingSystem.Models.ResponseModels.ImageResponse;
using CloudinaryDotNet.Actions;
using CloudinaryDotNet;
using Microsoft.Extensions.Options;
using Capstone_VotingSystem.Core.CoreModel;

namespace Capstone_VotingSystem.Services.CloudinaryService
{
    public class CloudinaryService : ICloudinaryService
    {
        private readonly Cloudinary _cloudinary;


        public CloudinaryService(IOptions<CloudinarySettings> config)
        {
            var acc = new Account(
            config.Value.CloundName,
            config.Value.ApiKey,
            config.Value.ApiSecret
        );

            _cloudinary = new Cloudinary(acc);
        }

        public async Task<ImageUploadResponse> AddImageAsync(IFormFile file, string folderName)
        {
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


            return new ImageUploadResponse
            {
                PublicId = uploadResult.PublicId,
                SecureUrl = uploadResult.SecureUrl.AbsoluteUri
            };
        }

        public async Task<DeletionResult> DeleteImage(string publicId)
        {
            var deletionParams = new DeletionParams(publicId)
            {
                ResourceType = ResourceType.Raw
            };
            var results = await _cloudinary.DestroyAsync(deletionParams);
            return results;
        }
    }
}
