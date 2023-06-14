namespace Capstone_VotingSystem.Models.RequestModels.CampaignRequest
{
    public class CreateCampaignRequest
    {
        public string? Title { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public bool? Visibility { get; set; }
        //public bool? Status { get; set; }
        public string? UserId { get; set; }
        public Guid? CategoryId { get; set; }
    }
}
