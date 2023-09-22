namespace Capstone_VotingSystem.Models.ResponseModels.CampaignResponse
{
    public class GetCampaignForAdminResponse
    {
        public Guid CampaignId { get; set; }
        public string Title { get; set; } = null!;
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string? Description { get; set; }
        public string Visibility { get; set; } = null!;
        public string? ImgUrl { get; set; }
        public string Process { get; set; } = null!;
        public bool IsApprove { get; set; }
        public bool Status { get; set; }
        public string UserId { get; set; } = null!;
        public Guid CategoryId { get; set; }
        public bool VisibilityCandidate { get; set; }
        public bool Representative { get; set; }
        public bool PublishTheResult { get; set; }
    }
}
