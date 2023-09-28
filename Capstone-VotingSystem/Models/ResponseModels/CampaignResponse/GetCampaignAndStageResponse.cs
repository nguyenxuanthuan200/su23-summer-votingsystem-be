using Capstone_VotingSystem.Models.ResponseModels.StageResponse;

namespace Capstone_VotingSystem.Models.ResponseModels.CampaignResponse
{
    public class GetCampaignAndStageResponse
    {
        public Guid CampaignId { get; set; }
        public string? Title { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Description { get; set; }
        public string Visibility { get; set; }
        public string? ImgUrl { get; set; }
        public string Process { get; set; }
        public int? TotalCandidate { get; set; }
        public bool IsApprove { get; set; }
        public string UserId { get; set; }
        public Guid CategoryId { get; set; }
        public bool VisibilityCandidate { get; set; }
        public bool PublishTheResult { get; set; }
        public List<GetStageResponse> Stage { get; set; }
    }
}
