namespace Capstone_VotingSystem.Models.RequestModels.CandidateRequest
{
    public class CreateListCandidateRequest
    {
        public Guid CampaignId { get; set; }
        public List<CreateCandidateRequest> listCandidate { get; set; }
    }
}
