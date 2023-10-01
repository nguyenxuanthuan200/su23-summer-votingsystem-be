namespace Capstone_VotingSystem.Models.RequestModels.RatioRequest
{
    public class CreateRatioRequest
    {
        public float Proportion { get; set; }
        public Guid GroupVoterId { get; set; }
        public Guid CampaignId { get; set; }
        public Guid GroupCandidateId { get; set; }
    }
}
