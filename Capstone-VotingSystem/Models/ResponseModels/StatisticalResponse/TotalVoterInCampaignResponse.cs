namespace Capstone_VotingSystem.Models.ResponseModels.StatisticalResponse
{
    public class TotalVoterInCampaignResponse
    {
        public Guid GroupId { get; set; }
        public string GroupName { get; set; }
        public int Total { get; set; }
    }
}
