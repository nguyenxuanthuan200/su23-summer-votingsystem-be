namespace Capstone_VotingSystem.Models.ResponseModels.StageResponse
{
    public class CreateStageResponse
    {
        public Guid StageId { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Content { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public Guid? CampaignId { get; set; }
        public Guid? FormId { get; set; }
    }
}
