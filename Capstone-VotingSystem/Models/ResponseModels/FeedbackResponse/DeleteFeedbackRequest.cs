namespace Capstone_VotingSystem.Models.ResponseModels.FeedbackResponse
{
    public class DeleteFeedbackRequest
    {
        public string? UserId { get; set; }
        public Guid? CampaignId { get; set; }
    }
}
