namespace Capstone_VotingSystem.Models.ResponseModels.CandidateResponse
{
    public class GetListCandidateStageResponse
    {
        public Guid? StageId { get; set; }
        public Guid? FormId { get; set; }
        public Guid? CampaignId { get; set; }

        public List<ListCandidateStageResponse> Candidate { get; set; }
        public List<ListCandidateVotedByUser> CandidateIsVoted { get; set; }
    }
}
