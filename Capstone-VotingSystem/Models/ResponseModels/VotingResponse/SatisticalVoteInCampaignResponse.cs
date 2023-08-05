namespace Capstone_VotingSystem.Models.ResponseModels.VotingResponse
{
    public class SatisticalVoteInCampaignResponse
    {
        public DateTime Date { get; set; }

        public int TotalVoteInCampaign {get; set; }
        public List<SatisticalVoteInStageResponse> VoteOfGroupInStage { get; set; }
        
    }
}
