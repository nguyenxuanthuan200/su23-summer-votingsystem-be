using Capstone_VotingSystem.Models.ResponseModels.ImageResponse;
using Capstone_VotingSystem.Services.CloudinaryService;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Capstone_VotingSystem.Controller
{
    [Route("api/v1/image")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private readonly ICloudinaryService _cloudinaryService;

        public ImageController(ICloudinaryService cloudinaryService)
        {
            _cloudinaryService = cloudinaryService;
        }

        [HttpPost]
        [SwaggerOperation(summary: "Upload Image")]
        public Task<ImageUploadResponse> UploadImage(IFormFile file)
        {
            var folderName = "campaign"; // Specify your folder name here

            var uploadResult = _cloudinaryService.AddImageAsync(file, folderName);

            return uploadResult;
        }
        [HttpDelete("{publicId}")]
        [SwaggerOperation(summary: "Delete Image")]
        public Task<DeletionResult> DeleteImageAsync(string publicId)
        {
            var uploadResult = _cloudinaryService.DeleteImage(publicId);
            return uploadResult;
        }
    }
}
