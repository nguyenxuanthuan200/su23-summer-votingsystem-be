namespace Capstone_VotingSystem.Models.RequestModels.CandidateRequest
{
    public class CreateCandidateCampaignRequest
    {
        public string? Description { get; set; }
        public string UserId { get; set; }
        public Guid CampaignId { get; set; }
    }
}
