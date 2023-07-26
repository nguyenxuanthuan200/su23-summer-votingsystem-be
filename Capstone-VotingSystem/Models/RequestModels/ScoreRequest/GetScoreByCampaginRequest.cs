namespace Capstone_VotingSystem.Models.RequestModels.ScoreRequest
{
    public class GetScoreByCampaginRequest
    {
        public Guid CampaignId { get; set; }
        public string UserId { get; set; }
        public string? Keyword { get; set; }
        public int? Page { get; set; } = 1;
        public int? PageSize { get; set; } = 10;
    }
}
