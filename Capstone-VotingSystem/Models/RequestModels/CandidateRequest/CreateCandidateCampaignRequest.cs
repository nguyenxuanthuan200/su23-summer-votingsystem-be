namespace Capstone_VotingSystem.Models.RequestModels.CandidateRequest
{
    public class CreateCandidateCampaignRequest
    {
        public Guid CampaignId { get; set; }
        public List<ListUserRequest> ListUser { get; set; }
    }
}
