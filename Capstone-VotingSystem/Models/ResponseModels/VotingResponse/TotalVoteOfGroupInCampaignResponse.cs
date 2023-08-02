namespace Capstone_VotingSystem.Models.ResponseModels.VotingResponse
{
    public class TotalVoteOfGroupInCampaignResponse
    {
        public Guid GroupId { get; set; }
        public string GroupName { get; set; }
        public int TotalVote { get; set; }
    }
}
