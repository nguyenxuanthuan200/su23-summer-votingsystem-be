namespace Capstone_VotingSystem.Models.ResponseModels.CampaignResponse
{
    public class GetCampaignResponse
    {
        public Guid CampaignId { get; set; }
        public string Title { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Visibility { get; set; }
        public string? ImgUrl { get; set; }
        public string? Process { get; set; }
        public int? TotalCandidate { get; set; }
        public bool IsApprove { get; set; }
        public string UserId { get; set; } = null!;
        public Guid CategoryId { get; set; }
        public bool VisibilityCandidate { get; set; }
        public bool PublishTheResult { get; set; }
    }
}
