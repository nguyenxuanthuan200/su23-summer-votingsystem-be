namespace Capstone_VotingSystem.Models.ResponseModels.RatioResponse
{
    public class RatioResponse
    {
        public Guid RatioGroupId { get; set; }
        public double? Proportion { get; set; }
        public Guid? GroupVoterId { get; set; }
        public string GroupNameOfVoter { get; set; }
        public Guid? CampaignId { get; set; }
        public Guid? GroupCandidateId { get; set; }
        public string GroupNameOfCandidate { get; set; }
    }
}
