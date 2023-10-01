namespace Capstone_VotingSystem.Models.RequestModels.VoteRequest
{
    public class StatisticalVoteRequest
    {
        public Guid CampaignId { get; set; }
        public DateTime DateAt { get; set; }
        public DateTime ToDate { get; set; }
    }
}
