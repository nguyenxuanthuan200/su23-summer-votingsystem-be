namespace Capstone_VotingSystem.Models.ResponseModels.VotingResponse
{
    public class SatisticalVoteInStageResponse
    {
        public Guid StageId { get; set; }
        public string StageName { get; set; }
        public int TotalVoteInStage { get; set; }
        public List<TotalVoteOfGroupInCampaignResponse> VoteOfGroup { get; set; }
    }
}
