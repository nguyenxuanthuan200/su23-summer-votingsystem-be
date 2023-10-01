namespace Capstone_VotingSystem.Models.RequestModels.FeedbackRequest
{
    public class FeedbackRequest
    {
        public string Content { get; set; }
        public string UserId { get; set; }
        public Guid CampaignId { get; set; }
    }

}

