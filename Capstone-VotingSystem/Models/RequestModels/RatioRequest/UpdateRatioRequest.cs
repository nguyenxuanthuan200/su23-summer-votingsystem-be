namespace Capstone_VotingSystem.Models.RequestModels.RatioRequest
{
    public class UpdateRatioRequest
    {
        public float Proportion { get; set; }
        public Guid GroupVoterId { get; set; }
        public Guid GroupCandidateId { get; set; }
    }
}
