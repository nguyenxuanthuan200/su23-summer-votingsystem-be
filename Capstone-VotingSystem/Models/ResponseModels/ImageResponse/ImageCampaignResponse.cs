namespace Capstone_VotingSystem.Models.ResponseModels.ImageResponse
{
    public class ImageCampaignResponse
    {
        public Guid? CampaignId { get; set; }

        public string? PublicId { get; set; }

        public string? SecureUrl { get; set; }
    }
}
