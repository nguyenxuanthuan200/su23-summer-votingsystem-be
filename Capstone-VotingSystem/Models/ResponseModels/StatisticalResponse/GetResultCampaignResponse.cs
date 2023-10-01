namespace Capstone_VotingSystem.Models.ResponseModels.StatisticalResponse
{
    public class GetResultCampaignResponse
    {
        public string FullName { get; set; }
        public string? Description { get; set; }
        public string GroupName { get; set; }
        public string AvatarUrl { get; set; }
        public int Score { get; set; }
    }
}
