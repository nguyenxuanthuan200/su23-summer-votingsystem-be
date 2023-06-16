using Capstone_VotingSystem.Controllers;
using Capstone_VotingSystem.Services.CloudinaryService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CoreApiResponse;

namespace Capstone_VotingSystem.Controller
{
    [Route("api/v1/image")]
    [ApiController]
    public class ImageController : BaseApiController
    {
        private readonly ICloudinaryService _cloudinary;

        public ImageController(ICloudinaryService cloudinaryService)
        {
            _cloudinary = cloudinaryService;
        }
        [HttpPut("userId")]
        public async Task<IActionResult> AddUserImage(IFormFile formFile, string? userId)
        {
            try
            {
                var folderName = "user";
                var result = await _cloudinary.AddImageUserAsync(formFile, folderName, userId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut("campaignId")]
        public async Task<IActionResult> AddCampaignImage(IFormFile formFile, Guid? campaignId)
        {
            try
            {
                var folderName = "campaign";
                var result = await _cloudinary.AddImageCampaignAsync(formFile, folderName, campaignId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
