namespace Capstone_VotingSystem.Models.ResponseModels.CandidateResponse
{
    public class GetListCandidateStageResponse
    {
        public Guid? StageId { get; set; }
        public string StageName { get; set; }
        public Guid? FormId { get; set; }
        public Guid? CampaignId { get; set; }
        public string CampaignName { get; set; }
        public string? Description { get; set; }
        public int LimitVoteOfStage { get; set; }
        public GetVoteResponse votesRemaining { get; set; }

        public List<ListCandidateStageResponse> Candidate { get; set; }
        //public List<ListCandidateVotedByUser> CandidateIsVoted { get; set; }
    }
}
