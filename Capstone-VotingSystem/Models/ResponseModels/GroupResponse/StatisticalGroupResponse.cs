namespace Capstone_VotingSystem.Models.ResponseModels.GroupResponse
{
    public class StatisticalGroupResponse
    {
        public int TotalVoterInCampaign { get; set; }
        public int TotalCandiadteInCampaign { get; set; }
        public List<StatisticalVoterInGroupResponse> StatisticalVoterInGroup { get; set; }
        public List<StatisticalCandidateInGroupResponse> StatisticalCandidateInGroup { get; set; }

    }
}
