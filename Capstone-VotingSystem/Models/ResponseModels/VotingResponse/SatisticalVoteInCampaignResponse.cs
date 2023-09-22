namespace Capstone_VotingSystem.Models.ResponseModels.VotingResponse
{
    public class SatisticalVoteInCampaignResponse
    {
       // public string Date { get; set; }

        public int TotalVoteInCampaign {get; set; }
        public int TotalVoteInCampaignByFilter { get; set; }
        public List<SatisticalVoteInStageResponse> VoteOfGroupInStage { get; set; }
        
    }
}
