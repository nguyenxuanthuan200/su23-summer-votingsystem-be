namespace Capstone_VotingSystem.Models.RequestModels.SearchRequest
{
    public class SearchCandidateRequest
    {
        public string? Keyword { get; set; }

        public int? Page { get; set; } = 1;
        public int? PageSize { get; set; } = 10;

        public Guid? GroupId { get; set; } // theo nhom
        public Guid? CampaignId { get; set; } // theo campaign
    }
}
