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
        [HttpPut("userid")]
        public async Task<IActionResult> AddUserImage(IFormFile formFile, string? userid)
        {
            try
            {
                var folderName = "user";
                var result = await _cloudinary.AddImageUserAsync(formFile, folderName, userid);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut("campaignid")]
        public async Task<IActionResult> AddCampaignImage(IFormFile formFile, Guid? campaignid)
        {
            try
            {
                var folderName = "campaign";
                var result = await _cloudinary.AddImageCampaignAsync(formFile, folderName, campaignid);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
