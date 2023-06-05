namespace Capstone_VotingSystem.Models.ResponseModels.CampaignStageResponse
{
    public class CreateCampaginStageResponse
    {
        public Guid CampaignStageId { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public bool? Status { get; set; }
        public string? Text { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public Guid? CampaignId { get; set; }
    }
}
