namespace Capstone_VotingSystem.Models.RequestModels.FeedbackRequest
{
    public class DeleteFeedbackRequest
    {
        public string? UserId { get; set; }
        public Guid? CampaignId { get; set; }
    }
}
