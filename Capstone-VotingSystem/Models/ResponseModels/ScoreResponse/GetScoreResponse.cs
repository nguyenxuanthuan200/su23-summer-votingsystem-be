namespace Capstone_VotingSystem.Models.ResponseModels.ScoreResponse
{
    public class GetScoreResponse
    {
        public Guid CampaignId { get; set; }
        public List<ListCandidateScoreResponse> listCandidateScore { get; set; }
    }
}
