namespace Capstone_VotingSystem.Models.RequestModels.CandidateRequest
{
    public class DeleteCandidateRequest
    {
        public string userId { get; set; }
        public Guid campaignId { get; set; }
    }
}
