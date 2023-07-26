namespace Capstone_VotingSystem.Models.ResponseModels.RatioResponse
{
    public class RatioResponse
    {
        public Guid RatioGroupId { get; set; }
        public decimal? Percent { get; set; }
        public Guid? GroupId { get; set; }
        public Guid? CampaignId { get; set; }
        public Guid? GroupCandidateId { get; set; }
    }
}
