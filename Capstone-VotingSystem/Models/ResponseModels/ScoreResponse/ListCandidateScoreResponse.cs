namespace Capstone_VotingSystem.Models.ResponseModels.ScoreResponse
{
    public class ListCandidateScoreResponse
    {
        public Guid CandidateId { get; set; }
        public string FullName { get; set; }
        public int? TotalScore { get; set; }
        public List<GetStageScoreResponse> listStageScore { get; set; }
    }
}
