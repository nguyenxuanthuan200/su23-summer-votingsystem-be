namespace Capstone_VotingSystem.Models.RequestModels.CandidateRequest
{
    public class CreateAccountCandidateRequest
    {
        public Guid CampaignId { get; set; }
        public List<ListAccountCandidateRequest> listAccountCandidate { get; set; }
    }
}
