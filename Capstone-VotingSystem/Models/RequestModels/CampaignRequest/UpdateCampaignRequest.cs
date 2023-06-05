namespace Capstone_VotingSystem.Models.RequestModels.CampaignRequest
{
    public class UpdateCampaignRequest
    {
        public Guid CampaignId { get; set; }
        public string? Title { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public bool? Status { get; set; }
        public bool? Visibility { get; set; }
    }
}
