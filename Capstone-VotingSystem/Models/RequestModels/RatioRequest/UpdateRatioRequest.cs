namespace Capstone_VotingSystem.Models.RequestModels.RatioRequest
{
    public class UpdateRatioRequest
    {
        public decimal? Percent { get; set; }
        public Guid? GroupId { get; set; }
        public Guid? CampaignId { get; set; }
        public Guid? GroupCandidateId { get; set; }
    }
}
