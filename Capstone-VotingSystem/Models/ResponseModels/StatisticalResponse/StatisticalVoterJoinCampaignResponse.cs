using Capstone_VotingSystem.Models.ResponseModels.VotingResponse;

namespace Capstone_VotingSystem.Models.ResponseModels.StatisticalResponse
{
    public class StatisticalVoterJoinCampaignResponse
    {
        public List<TotalVoterInCampaignResponse> GroupOfVoter { get; set; }
        public List<TotalVoterInCampaignResponse> GroupMajorOfVoter { get; set; }
    }
}
